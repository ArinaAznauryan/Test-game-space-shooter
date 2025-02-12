using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }
    public Player player;
    [SerializeField] public ObjectPoolingManager pool;

    [SerializeField] private Vector3 _spawnPosition;
    ///[SerializeField] private Vector3 _spawnOffsets;

    private float screenBoundX, screenBoundY;
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
        DontDestroyOnLoad(gameObject); // Optional, if it should persist across scenes

        // pool = GetComponent<ObjectPoolingManager>();

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
            _nextSpawnTime = Time.time + _enemySpawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        var enemy = pool.Enemy.Spawn();
        if (enemy is null)
        {
            Debug.LogError("Something is wrong with enemy spawning and despawning!");
            return;
        }

        enemy.transform.position = _spawnPosition + new Vector3(
              Random.Range(-GetEnemyScreenBoundsX(), GetEnemyScreenBoundsX()),
              Random.Range(0f, 0f),
              //Random.Range(-_spawnOffsets.y, _spawnOffsets.y),
              0.0f
        );
        
    }


    void OnPlayerDie() {
        _running = false;
    }


}
