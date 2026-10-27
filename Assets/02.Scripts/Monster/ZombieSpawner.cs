using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _zombiePrefab;

    [Header("Spawn settings")]
    [SerializeField, Min(0f)] private float _spawnInterval = 3f;
    [SerializeField, Min(0f)] private float _innerRadius = 8f;
    [SerializeField, Min(0f)] private float _outerRadius = 15f;
    [SerializeField, Min(1)] private int _maxAliveZombies = 10;
    [SerializeField] private bool _spawnImmediately = true;

    private float _spawnTimer;

    private void Awake()
    {
        if (_player == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) _player = player.transform;
        }
    }

    private void Start()
    {
        _spawnTimer = _spawnImmediately ? 0f : Mathf.Max(0.05f, _spawnInterval);
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameState != GameState.Playing)
            return;

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer > 0f) return;

        _spawnTimer = Mathf.Max(0.05f, _spawnInterval);
        TrySpawnZombie();
    }

    private void TrySpawnZombie()
    {
        if (_player == null || _zombiePrefab == null)
        {
            Debug.LogWarning("ZombieSpawner: 플레이어 또는 좀비 프리팹이 지정되지 않았습니다.", this);
            return;
        }

        if (FindObjectsByType<Zombie>(FindObjectsSortMode.None).Length >= _maxAliveZombies)
            return;

        float minRadius = Mathf.Min(_innerRadius, _outerRadius);
        float maxRadius = Mathf.Max(_innerRadius, _outerRadius);
        float radius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        Vector3 spawnPosition = _player.position + offset;
        Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        var zombieObject = Instantiate(_zombiePrefab, spawnPosition, spawnRotation);
        var zombie = zombieObject.GetComponent<Zombie>();
        var remainEnemyText = FindFirstObjectByType<RemainEnemyText>();

        if (zombie != null && remainEnemyText != null)
            remainEnemyText.RegisterZombie(zombie);
    }
}
