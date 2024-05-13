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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawningEnemy()
    {
        co_SpawnEnemy = StartCoroutine(Co_SpawnEnemy());
    }

    IEnumerator Co_SpawnEnemy()
    {
        int j = 0;
        while(true)
        {
            Enemy enemyPrefab = enemyPrefabs[j % enemyPrefabs.Length];
            int n = Random.Range(1, 3);
            for (int i = 0; i < n; i++)
            {
                yield return new WaitForSeconds(3);
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemy.OnEnemyDied.AddListener(RemoveEnemy);
                enemy.OnEnemyKilled.AddListener(ScoreManager.Instance.IncreaseScore);
                enemiesSpawned.Add(enemy);
            }
            j++;
            yield return new WaitForSeconds(5);
            yield return new WaitWhile(TooManyEnemies);
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
