using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Player player;

    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    private Coroutine co_SpawnEnemy;

    private List<Enemy> enemiesSpawned = new List<Enemy>();

    [SerializeField] private AudioSource nukeAudioSource;

    private int[] inds; // for spawning enemy waves
    [SerializeField] private int nRound = 5; // rounds of random enemy spawn before waves
    [SerializeField] private int nSpawn = 3; // initial number of enemies to spawn before waves
    [SerializeField] private float initialInterval = 4; // initial time interval between spawning enemies
    [SerializeField] private float minInterval = 1; // minimum time interval between spawning enemies
    [SerializeField] private float tBetweenWaves = 6; // time interval between enemy waves

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartSpawningEnemy();

        player = FindObjectOfType<Player>();
        player.OnPlayerDied.AddListener(EndGame);
        player.OnNukeHappened.AddListener(Nuke);

        inds = new int[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            inds[i] = i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawningEnemy()
    {
        co_SpawnEnemy = StartCoroutine(Co_SpawnEnemy());
    }

    private void InstantiateEnemy(Enemy enemyPrefab, Vector3 pos)
    {
        Enemy enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemy.OnEnemyDied.AddListener(RemoveEnemy);
        enemy.OnEnemyKilled.AddListener(ScoreManager.Instance.IncreaseScore);
        enemiesSpawned.Add(enemy);
    }

    IEnumerator Co_SpawnEnemy()
    {
        yield return new WaitForSeconds(3);

        // randomly spawn enemies
        float dt = (initialInterval - minInterval) / nRound;
        for (int n = 0; n < nRound; n++)
        {
            for (int i = 0; i < nSpawn; i++)
            {
                Enemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                InstantiateEnemy(enemyPrefab, spawnPoint.position);
                yield return new WaitForSeconds(initialInterval - dt * n); // gradually decrease the time interval
            }
            yield return new WaitForSeconds(tBetweenWaves);
            yield return new WaitWhile(TooManyEnemies);
        }

        // waves
        int j = 0;
        while(true)
        {
            Enemy enemyPrefab = enemyPrefabs[j % enemyPrefabs.Length];
            int n = 3 + j;//Random.Range(3, spawnPoints.Length + j);
            n = Mathf.Clamp(n, 3, spawnPoints.Length);
            GenerateEnemyWave(enemyPrefab, n);
            j++;
            yield return new WaitForSeconds(tBetweenWaves);
            yield return new WaitWhile(TooManyEnemies);
        }
    }

    private void GenerateEnemyWave(Enemy enemyPrefab, int n)
    {
        List<int> inds1 = RandomChoice.RandomSample(inds, n);
        foreach (int i in inds1)
        {
            Transform spawnPoint = spawnPoints[i];
            InstantiateEnemy(enemyPrefab, spawnPoint.position);
        }
    }

    public void EndGame()
    {
        StopCoroutine(co_SpawnEnemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemiesSpawned.Remove(enemy);
    }

    private bool TooManyEnemies()
    {
        return enemiesSpawned.Count >= 20;
    }

    private void Nuke()
    {
        foreach (var enemy in enemiesSpawned)
        {
            ParticleManager.Instance.GetNukeParticles(enemy.transform.position);
            Destroy(enemy.gameObject);
            ScoreManager.Instance.IncreaseScore();
        }
        enemiesSpawned.Clear();

        PickUp[] pickUps = FindObjectsOfType<PickUp>();
        foreach (var item in pickUps)
        {
            ParticleManager.Instance.GetNukeParticles(item.transform.position);
            Destroy(item.gameObject);
        }

        nukeAudioSource.Play();
    }

    public void Reset()
    {
        player.gameObject.SetActive(true);
        player.Reset();

        foreach (var enemy in enemiesSpawned)
        {
            Destroy(enemy.gameObject);
        }
        enemiesSpawned.Clear();

        PickUp[] pickUps = FindObjectsOfType<PickUp>();
        foreach (var item in pickUps)
        {
            Destroy(item.gameObject);
        }

        StartSpawningEnemy();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll(); // Clears all PlayerPrefs data
    }
}
