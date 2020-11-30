using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    [SerializeField]
    private GameObject poolingEnemyPrefab;

    private Queue<Enemy> poolingEnemyQueue = new Queue<Enemy>();
    

    private Enemy CreateNewEnemy()
    {
        var newObj = Instantiate(poolingEnemyPrefab, transform).GetComponent<Enemy>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    public void InitEnemyPool(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            poolingEnemyQueue.Enqueue(CreateNewEnemy());
        }
    }

    public Enemy GetEnemy()
    {
        if(poolingEnemyQueue.Count > 0)
        {
            var obj = poolingEnemyQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var obj = CreateNewEnemy();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }

}
