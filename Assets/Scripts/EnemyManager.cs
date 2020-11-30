using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance = null;

    [SerializeField]
    private int enemyCount = 100;

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

    void init()
    {
        if (pool != null)
            pool.InitEnemyPool(enemyCount);
    }

}
