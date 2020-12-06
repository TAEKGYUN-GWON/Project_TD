using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    private int maxEnemyCount = 0;
    [SerializeField]
    private int curEnemyCount = 0;
    [SerializeField]
    private int maxSpawnCount = 0;
    [SerializeField]
    private float spawnWaitTime = 0;
    [SerializeField]
    private float nextSpawnTime = 0;

    public Transform spawnPoint;

    private IEnumerator _waveLoutine;

    ObjectBasePool _enemyPool;

    InGameManager _inGameManager;

    [SerializeField]
    Enemy enemyPrefab;

    void Awake()
    {
        Init();
        _waveLoutine = SpawnWaveRootin();
        StartCoroutine(_waveLoutine);
    }

    List<Dictionary<string, object>> _listWaveInfo = new List<Dictionary<string, object>>();
    List<Dictionary<string, object>> _listEnemyInfo = new List<Dictionary<string, object>>();

    void Init()
    {
        _inGameManager = InGameManager.Instance;
        _inGameManager.IsWaveActive = true;

        _listWaveInfo = TableManager.Instance.GetTable("info_enemy_wave");
        _listEnemyInfo = TableManager.Instance.GetTable("info_enemy");

        maxSpawnCount = _listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnCount"].GetHashCode();
        maxEnemyCount = _listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnMaxCount"].GetHashCode();
        spawnWaitTime = (float)_listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnWaitTime"];
        nextSpawnTime = (float)_listWaveInfo[InGameManager.Instance.gameLevel - 1]["NextSpawnTime"];

        _enemyPool = ObjectBasePool.CreateInstancePool(transform, enemyPrefab, 100, true, 100);
    }

    IEnumerator SpawnWaveRootin()
    {
        while (true)
        {
            if (!_inGameManager.IsWaveActive)
                break;

            StartCoroutine(SpawnWave());
            yield return OverStory.YieldInstructionCache.WaitForSeconds(nextSpawnTime);
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < maxSpawnCount; i++)
        {
            curEnemyCount++;
            SpawnEnemy();

            if (curEnemyCount >= maxEnemyCount)
            {
                LevelUp();
                break;
            }

            yield return OverStory.YieldInstructionCache.WaitForSeconds(spawnWaitTime);
        }
    }

    void LevelUp()
    {
        InGameManager.Instance.gameLevel++;
        if(InGameManager.Instance.gameLevel > _listWaveInfo.Count)
        {
            InGameManager.Instance.gameLevel = 0;
        }
        maxEnemyCount = _listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnMaxCount"].GetHashCode();
        maxSpawnCount = _listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnCount"].GetHashCode();
        spawnWaitTime = (float)_listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnWaitTime"];
        nextSpawnTime = (float)_listWaveInfo[InGameManager.Instance.gameLevel - 1]["NextSpawnTime"];
        curEnemyCount = 0;
    }

    void SpawnEnemy()
    {
        var enemy = _enemyPool.Spawn<Enemy>();
        enemy.speed = _listEnemyInfo[1]["Speed"].GetHashCode();
        var enemyTransform = enemy.gameObject.transform;
        enemyTransform.position = spawnPoint.position;
        enemyTransform.rotation = spawnPoint.rotation;
        enemy.gameObject.transform.SetParent(transform);
    }

}
