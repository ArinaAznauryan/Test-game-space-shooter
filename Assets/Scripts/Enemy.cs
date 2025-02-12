using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

struct RandomSelection
{
    private int minValue;
    private int maxValue;
    public float probability;

    public RandomSelection(int minValue, int maxValue, float probability)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.probability = probability;
    }

    public int GetValue() { return Random.Range(minValue, maxValue + 1); }
}



public enum EnemyMovementType
{
    STRAIGHT,
    ZIGZAG
}

public class Enemy : MonoBehaviour {

    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private PowerUp _prefabPowerUp;
    [SerializeField] private Projectile _prefabProjectile;

    private EnemyMovement movement;

    private float _powerUpSpawnChance = 0.1f;
    private int _health = 2;
    private Rigidbody _body;

    private bool canFire = false;
    public float _fireInterval = 2.5f;
    private float _fireTimer = 0.0f;


    private GameplayUi _gameplayUI;

    private void Awake() {
        _body = GetComponent<Rigidbody>();
        

        _gameplayUI = Object.FindObjectOfType<GameplayUi>(true);


        _health = 2 + Mathf.Min(Mathf.FloorToInt(Time.time / 15f), 5);
    }

    private void OnEnable()
    {
        canFire = Random.value < 0.6f;

        InitMovement();
    }

    private void InitMovement()
    {
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<EnemyMovement>().enabled = true;
        movement = GetComponent<EnemyMovement>();
    }

    void Update() {

        if (canFire) {

            if (Time.time >= _fireTimer + _fireInterval)
            {
                Debug.Log("Fire an enemy projectile!");
                FireProjectile();
                _fireTimer = Time.time;
            }
        }

        
    }
    
    private void FireProjectile()
    {
        var go = GameController.Instance.pool.EnemyProjectile.Spawn();
        go.transform.position = transform.position;
        go.GetComponent<Projectile>().Init(1, movement.GetSpeed());
    }

    private void FixedUpdate() {
        movement.Move();
    }


    public void Hit(int damage) {
        _health -= damage;

        if (_health <= 0) {

            var fx = GameController.Instance.pool.EnemyProjectile.Spawn();
            fx.transform.position = transform.position;

            if (Random.value < _powerUpSpawnChance) SpawnPowerUp();

            _gameplayUI.AddScore(1);

            GameController.Instance.pool.EnemyProjectile.Despawn(fx);
            GameController.Instance.pool.Enemy.Despawn(gameObject);

        }
    }

    private void SpawnPowerUp()
    {
        var powerUp = Instantiate(_prefabPowerUp);

        var randomType = (PowerUp.PowerUpType)Random.Range(0, Enum.GetValues(typeof(PowerUp.PowerUpType)).Length);
        powerUp.SetType(randomType);
    }

}
