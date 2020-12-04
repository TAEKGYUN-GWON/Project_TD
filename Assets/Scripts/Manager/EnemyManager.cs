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

    private IEnumerator waveLoutine;

    ObjectBasePool enemyPool;

    [SerializeField]
    Enemy enemyPrefab;

    bool isWave = false;

    void Awake()
    {
        init();
        waveLoutine = SpawnWaveRootin();
        StartCoroutine(waveLoutine);
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {
    }

    List<Dictionary<string, object>> listWaveInfo = new List<Dictionary<string, object>>();
    List<Dictionary<string, object>> listEnemyInfo = new List<Dictionary<string, object>>();

    void init()
    {
        isWave = true;

        listWaveInfo = TableManager.Instance.GetTable("info_enemy_wave");
        listEnemyInfo = TableManager.Instance.GetTable("info_enemy");

        maxSpawnCount = listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnCount"].GetHashCode();
        maxEnemyCount = listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnMaxCount"].GetHashCode();
        spawnWaitTime = (float)listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnWaitTime"];
        nextSpawnTime = (float)listWaveInfo[InGameManager.Instance.gameLevel - 1]["NextSpawnTime"];

        enemyPool = ObjectBasePool.CreateInstancePool(transform, enemyPrefab, 100, true, 100);
    }

    IEnumerator SpawnWaveRootin()
    {
        while (true)
        {
            if (!isWave)
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
                LevelUP();
                break;
            }

            yield return OverStory.YieldInstructionCache.WaitForSeconds(spawnWaitTime);
        }
    }

    void LevelUP()
    {
        InGameManager.Instance.gameLevel++;
        if(InGameManager.Instance.gameLevel > listWaveInfo.Count)
        {
            isWave = false;
            return;
        }
        maxEnemyCount = listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnMaxCount"].GetHashCode();
        maxSpawnCount = listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnCount"].GetHashCode();
        spawnWaitTime = (float)listWaveInfo[InGameManager.Instance.gameLevel - 1]["SpawnWaitTime"];
        nextSpawnTime = (float)listWaveInfo[InGameManager.Instance.gameLevel - 1]["NextSpawnTime"];
        curEnemyCount = 0;
    }

    void SpawnEnemy()
    {
        var enemy = enemyPool.Spawn<Enemy>();
        enemy.speed = listEnemyInfo[1]["Speed"].GetHashCode();
        var enemyTransform = enemy.gameObject.transform;
        enemyTransform.position = spawnPoint.position;
        enemyTransform.rotation = spawnPoint.rotation;
        enemy.gameObject.transform.SetParent(transform);
    }

}
