using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using PrimeTween;
using PrimeTweenDemo;
using static MyTools.Tools;


public class Player : MonoBehaviour {

    public event System.Action OnDie;

    public GameObject tripleRocket; //Prefab of tripleRocket version of the player

    private bool canFire = true; //If the player can fire projectiles
    
    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private Projectile _prefabProjectile;
    [SerializeField] protected Transform _projectileSpawnLocation;

    private int _health = 3;

    private Rigidbody _body = null;
    
    private Vector2 _lastInput;
    private bool _hasInput = false;
    
    float _fireInterval = 0.4f;
    private float _fireTimer = 0.0f;

    private Vector3 previousPosition = new Vector3(0f, 0f, 0f);
    private float velocity = 0f;
    private float angle = 0f;

    // Player accelerated movement properties
    private float acceleration;
    private float previousSpeed;
    private float rotationForce = 30f;
    private float maxAngle = 60f;
    private float returnSpeed = 10f;
    private float velocityMultiplier = 20f;
    private float floatSpeed = 5f;
    private float floatHeight = .2f;
    // Player accelerated movement properties

    public int _fireDamage = 1;

    private GameplayUi _gameplayUI;
    private GameOverUi _gameOverUI;

    private bool vulnerable = true; //If the player can get hit by enemies

    protected virtual void OnEnable()
    {
        ResetDamage();
        UpdateHealth(3);

        //During player version switching (default or triple rocket),
        //checks for the power-ups that were still active on the recent active player and puts them on the swithced player
        CheckForPowerUps();
    }

    protected virtual void OnDisable()
    {
        //Putting active power-ups on the switching verion of the player during disabling the current one
        Invoke(nameof(AdjustForPowerUps), 0.01f); 
    }

    public void MakeVulnerable(bool yesOrNo)
    {
        vulnerable = yesOrNo;
    }

    protected void CheckForPowerUps()
    {
        //The only power-up that has to get manually moved
        FindObjectOfType<ProtectiveField>()?.AdjustToPlayer();
    }

    protected void AdjustForPowerUps()
    {
        ProtectiveField protectiveField = FindObjectOfType<ProtectiveField>(true);
        if (protectiveField.transform.parent == transform) protectiveField.transform.parent = null;
    }

    private void Awake() {
        _body = GetComponent<Rigidbody>();
        _gameplayUI = Object.FindObjectOfType<GameplayUi>(true);
        _gameOverUI = Object.FindObjectOfType<GameOverUi>(true);
        tripleRocket = FindObjectOfType<PlayerTripleRocket>(true).gameObject;

        UpdateHealth(3);
    }

    void Start() {
        _gameOverUI.Close();
    }

    protected void AssignInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) _hasInput = true;
        if (Input.touchCount == 0 || !Input.GetMouseButton(0)) _hasInput = false;

        if (_hasInput)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) _lastInput = Input.mousePosition;
            
        }
    }

    private void StartFire()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireInterval)
        {
            FireProjectile();
            _fireTimer -= _fireInterval;
        }
    }

    protected virtual void FireProjectile()
    {
        if (!canFire) return;

        var go = GameController.Instance.pool.PlayerProjectile.Spawn(); //Spawning(pooling) a player projectile
       
        go.transform.position = _projectileSpawnLocation.position; //Alligning its position to the player's

        go.GetComponent<Projectile>().Init(5, _fireDamage); //Initializing its speed and damage it can cause to enemies
    }


    public void SetFireDamage(int damage)
    {
        _fireDamage = damage;
    }

    public void Reset()
    {
        FindObjectOfType<ProtectiveField>(true).transform.parent = null; //Resetting the complicated power-up
        ResetDamage();
        UpdateHealth(3);
    }
    public void ResetDamage() { _fireDamage = 1; }

    protected virtual void Update() {

        AssignInput();
        if (canFire) StartFire();
    }

    public void Hit(float damage) {
        if (!vulnerable) return;

        UpdateHealth(_health - (int)damage);


        if (_health <= 0)
        {
            Die();
            return;
        }

        PlayHit();
    }

    private void PlayHit()
    {
        //This tween will hit the player to the down direction and shake it
        var punchDir = -transform.up;

        Tween.PunchLocalPosition(transform, strength: punchDir, duration: .7f, frequency: 5f);
        StartCoroutine(HandlePlayerHit());
    }

    private IEnumerator HandlePlayerHit()
    {
        canFire = false;
        ColliderOn(false); //So during the player's disactiveness, it doesn't accidentally trigger power ups or enemies

        Instantiate(_prefabExplosion, transform.position, Quaternion.identity); //Explode
        
        // Flash effect: make the player scale down a bit to simulate the hit
        Tween.Scale(transform, endValue: 0f, duration: .5f);

        // Make the player disappear (alpha = 0)
        GetComponentInChildren<Renderer>().material.SetFloat("_Transparency", 0);

        // Wait for 2 seconds (Player stays invisible)
        GameController.Instance.StartCountdown();
        yield return new WaitForSeconds(3f);

        // Make the player reappear
        Tween.Scale(transform, endValue: 1f, duration: 1);
        GetComponentInChildren<Renderer>().material.SetFloat("_Transparency", 1); // Reset transparency

        transform.position = new Vector3(0f, 0f, 0f); // Set to initial posiGtion or wherever I want
        yield return new WaitForSeconds(0f);

        canFire = true;
        ColliderOn(true);
    }

    private void ColliderOn(bool onOrOff)
    {
        GetComponent<Collider>().enabled = onOrOff;
    }

    protected void UpdateHealth(int health)
    {
        _health = health;
        _gameplayUI.UpdateHealth(_health);
    }
    public void AddHealth()
    {
        if (_health >= 3) return; //Be able to add health only when it's lower than 3
        _health++;
        _gameplayUI.UpdateHealth(_health);
    }

    private void Die() {
        _gameOverUI.Open();
        Instantiate(_prefabExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        OnDie?.Invoke();
    }


    //For better movement look
    private void FloatInSpace() {
        angle = Mathf.Lerp(angle, 0, Time.fixedDeltaTime * returnSpeed);
        float yPos = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void MovePlayer() {
        Vector2 pos = _lastInput;
        const float playAreaMin = -3f;
        const float playAreaMax = 3f;

        var p = Mathf.Clamp01(pos.x / Screen.width);
        Vector3 targetPosition = new Vector3(Mathf.Lerp(playAreaMin, playAreaMax, p), 0.0f, 0.0f);

        velocity = (targetPosition.x - previousPosition.x) / Time.fixedDeltaTime;
        angle = Mathf.Clamp((targetPosition.x - previousPosition.x) * rotationForce + velocity * velocityMultiplier, -1 * maxAngle, maxAngle);

        _body.MovePosition(targetPosition);

        float currentSpeed = (targetPosition.x - previousPosition.x) / Time.fixedDeltaTime;
        acceleration = (currentSpeed - previousSpeed) / Time.fixedDeltaTime;
        previousSpeed = currentSpeed;
        previousPosition = targetPosition;
    }

    private void Rotate()
    {
        float smoothedTilt = Mathf.LerpAngle(_body.rotation.eulerAngles.z, -1 * angle, Time.deltaTime * 5f);

        _body.MoveRotation(Quaternion.Euler(0, 0, smoothedTilt));

        if (Mathf.Abs(angle) < 0.1f) angle = Mathf.Lerp(angle, 0, Time.deltaTime * returnSpeed);

    }

    private void FixedUpdate() {
        if (!_hasInput) FloatInSpace(); //If the player is still (idle), it will float in space

        else MovePlayer();

        Rotate();
    }

    void TurnIntoTripleRocket()
    {
        if (GetComponent<PlayerTripleRocket>()) return;
        tripleRocket.SetActive(true);
        tripleRocket.transform.position = transform.position;
        gameObject.SetActive(false); //Disables the default player and enabled the triple rocket player verison
    }

    void ActivateProtectiveField()
    {
        FindObjectOfType<ProtectiveField>(true).gameObject.SetActive(true);
    }

    void SwitchState(PowerUp.PowerUpType newState)
    {
        switch (newState)
        {
            case PowerUp.PowerUpType.TRIPLE_ROCKET:
                TurnIntoTripleRocket();
                break;
            case PowerUp.PowerUpType.FIRE_RATE:
                _fireInterval *= 0.9f;
                break;
            case PowerUp.PowerUpType.PROTECTIVE_FIELD:
                ActivateProtectiveField();
                break;
            default: break;
        }
    }

    public void AddPowerUp(PowerUp.PowerUpType type) {
        
        SwitchState(type);
    }
}