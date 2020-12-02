using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance = null;

    private int enemyCount = 0;

    [SerializeField]
    private EnemyPool pool;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            instance.init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    List<Dictionary<string, object>> listWaveInfo = new List<Dictionary<string, object>>();

    void init()
    {

        listWaveInfo = TableManager.Instance.GetTable("info_enemy_wave");

        enemyCount = listWaveInfo[InGameManager.Instance.gameLevel - 1]["Count"].GetHashCode();

        if (pool != null)
            pool.InitEnemyPool(enemyCount);
    }

}
