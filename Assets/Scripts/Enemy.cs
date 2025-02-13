using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public enum EnemyMovementType
{
    STRAIGHT,
    ZIGZAG
}

public class Enemy : MonoBehaviour {

    [SerializeField] private GameObject _prefabExplosion;

    private EnemyMovement movement; //This is a separate component that needs to be on the enemy's gameobject, it controls its movement entirely

    private float _powerUpSpawnChance = 0.25f;
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
        GetComponent<EnemyMovement>().enabled = false; //Resetting the EnemyMovement properties
        GetComponent<EnemyMovement>().enabled = true;
        movement = GetComponent<EnemyMovement>();
    }

    void Update() {

        if (canFire) {

            if (Time.time >= _fireTimer + _fireInterval)
            {
                FireProjectile();
                _fireTimer = Time.time;
            }
        }

        
    }
    
    private void FireProjectile()
    {
        StartCoroutine(ShootAnimation()); //Playing enemy shooting animation (Animation component required on the enemy)
        var go = GameController.Instance.pool.EnemyProjectile.Spawn();
        go.transform.position = transform.position;
        go.GetComponent<EnemyProjectile>().Init(movement.GetSpeed());
    }

    private void FixedUpdate() {
        movement.Move();
    }

    private IEnumerator ShootAnimation()
    {
        GetComponent<Animation>().enabled = true;
        yield return new WaitForSeconds(50f/60f);
        GetComponent<Animation>().enabled = false;
    }

    public void Hit(int damage) {
        _health -= damage;

        if (_health <= 0) {

            if (Random.value < _powerUpSpawnChance) SpawnPowerUp();

            Die();

        }
    }

    public void Die()
    {
        _gameplayUI.AddScore(1);

        Instantiate(_prefabExplosion, transform.position, Quaternion.identity);
        GameController.Instance.pool.Enemy.Despawn(gameObject);
    }

    private void SpawnPowerUp()
    {
        var powerUp = GameController.Instance.pool.PowerUp.Spawn();
        powerUp.transform.position = new Vector3(Random.Range(-4f, 4f), 14f, 0.0f); //Spawns a power up randomly on the X axis

        //Chooses a random type of the power-up
        var randomType = (PowerUp.PowerUpType)Random.Range(0, Enum.GetValues(typeof(PowerUp.PowerUpType)).Length);
        powerUp.GetComponent<PowerUp>().SetType(randomType);
    }

}
