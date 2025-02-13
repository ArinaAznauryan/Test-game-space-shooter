using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using static MyTools.Tools;
using TMPro;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }
    public Player player;
    [SerializeField] public ObjectPoolingManager pool;

    [SerializeField] private Vector3 _spawnPosition;

    private float screenBoundX, screenBoundY;
    [SerializeField] private GameObject countdown;

    [SerializeField] public float _enemySpawnInterval = 0.5f;

    private float _nextSpawnTime;
    private bool _running = true;
    private Player _player;

    public float enemyWidth = 2.2f;

    private void FindScreenBounds()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;

        screenBoundX = screenWidth / 2f;
        screenBoundY = screenHeight / 2f;
    }

    public float GetEnemyScreenBoundsX()
    {
        return screenBoundX - (enemyWidth / 2f);
    }

    public float GetScreenBoundsX()
    {
        return screenBoundX;
    }

    public float GetScreenBoundsY()
    {
        return screenBoundY;
    }

    void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Application.targetFrameRate = 60;

        FindScreenBounds();

        player = GameObject.FindWithTag("MainPlayer").GetComponent<Player>();
    }

    void Start() {
        _player = FindObjectOfType<Player>(true);
        if (_player != null) {
            _player.OnDie += OnPlayerDie;
        }

        _running = true;
        
        _nextSpawnTime = Time.time + _enemySpawnInterval;
    }

    void Update() {

        if (!_running) return;

        if (Time.time >= _nextSpawnTime)
        {
            SpawnEnemy();
            SpawnHealPickUp();

            _nextSpawnTime = Time.time + _enemySpawnInterval;
            
            //Assigns the probability of enemies getting spawned more in a specific interval, with possibilities
            //for example spawning from .4 to .59 interval - 20 percent chance
            _enemySpawnInterval = GetRandomValueFloat(
                new RandomSelection(.4f, .59f, .2f),
                new RandomSelection(.6f, .89f, .4f),
                new RandomSelection(.9f, 1f, .8f)
            ); 
        }
    }

    private void SpawnHealPickUp()
    {
        if (Random.value > .15f) return; //Make sure to spawn it with the possibility of 15%

        var heal = pool.HealPickUp.Spawn();
        heal.transform.position = new Vector3(Random.Range(-4f, 4f), 14f, 0.0f); //Spawn in random places of X axis
    }

    private void SpawnEnemy()
    {
        var enemy = pool.Enemy.Spawn();

        //Spawns the enemy in desirable random places, taking account its sizes,
        //so its one part of the body doesn't get spawned outside the screen boundaries
        enemy.transform.position = _spawnPosition + new Vector3(
              Random.Range(-GetEnemyScreenBoundsX(), GetEnemyScreenBoundsX()),
              Random.Range(0f, 0f),
              0.0f
        );
        
    }

    public void StartCountdown()
    {  
        StartCoroutine(Countdown());
    }

    //Runs the countdown animation
    private IEnumerator Countdown()
    {
        GameObject countdown = FindObjectOfType<GameplayUi>(true).GetCountdown(); 
        countdown.SetActive(true);
        yield return new WaitForSeconds(3f);
        countdown.SetActive(false);
    }

    void OnPlayerDie() {
        _running = false;
    }


}
