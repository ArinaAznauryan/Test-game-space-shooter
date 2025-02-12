using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


//public enum PlayerState {DEFAULT, TRIPLE_ROCKET, DOUBLE_SHOOT, BOMB_SHOOT, PROTECTIVE_FIELD}

public class Player : MonoBehaviour {

    public event System.Action OnDie;

    public GameObject tripleRocket;
    
    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private Projectile _prefabProjectile;
    [SerializeField] protected Transform _projectileSpawnLocation;

    private int _health = 3;

    private float powerUpInterval = 5f;
    //public PlayerState state = PlayerState.DEFAULT;
    
    private Rigidbody _body = null;
    
    private Vector2 _lastInput;
    private bool _hasInput = false;
    
    float _fireInterval = 0.4f;
    private float _fireTimer = 0.0f;

    private Vector3 previousPosition = new Vector3(0f, 0f, 0f);
    private float velocity = 0f;
    private float angle = 0f;

    private float acceleration;
    private float previousSpeed;
    public float rotationForce = 5f;
    public float maxAngle = 30f;
    public float returnSpeed = 2f;
    public float velocityMultiplier = 10f;
    public float floatSpeed = 1f;
    public float floatHeight = 1f;

    private GameplayUi _gameplayUI;
    private GameOverUi _gameOverUI;


    private void Awake() {
        _body = GetComponent<Rigidbody>();
        _gameplayUI = Object.FindObjectOfType<GameplayUi>(true);
        _gameOverUI = Object.FindObjectOfType<GameOverUi>(true);
        tripleRocket = FindObjectOfType<PlayerTripleRocket>(true).gameObject;
    }

    void Start() {
        _gameplayUI.UpdateHealth(_health);
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
       
        var go = GameController.Instance.pool.PlayerProjectile.Spawn();
       
        go.transform.position = _projectileSpawnLocation.position;
    }

    protected virtual void Update() {

        AssignInput();
        StartFire();
    }

    public void Hit() {
        _health--;
        _gameplayUI.UpdateHealth(_health);

        if (_health <= 0) Die();
    }

    private void Die() {
        _gameOverUI.Open();
        Instantiate(_prefabExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        OnDie?.Invoke();
    }

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
        if (!_hasInput) FloatInSpace();

        else MovePlayer();

        Rotate();
    }

    void TurnIntoTripleRocket()
    {
        if (GetComponent<PlayerTripleRocket>()) return;
        tripleRocket.SetActive(true);
        tripleRocket.transform.position = transform.position;
        gameObject.SetActive(false);
    }

    void SwitchState(PowerUp.PowerUpType newState)
    {
       // state = newState;
        switch (newState)
        {
            //case PlayerState.DEFAULT:
            //    gameObject.SetActive(false);
            //    GameController.Instance.player.gameObject.SetActive(true);
            //    break;
            case PowerUp.PowerUpType.TRIPLE_ROCKET:
                TurnIntoTripleRocket();
                break;
            case PowerUp.PowerUpType.FIRE_RATE:
                _fireInterval *= 0.9f;
                break;
        }
    }

    public void AddPowerUp(PowerUp.PowerUpType type) {
        
        SwitchState(type);
    }
}