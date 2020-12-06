using System;
using System.Linq;
using Unity.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    public float _range = 15f;

    public string enemyTag = "Enemy";
    
    [SerializeField]
    private Transform _partToRotate;

    public float turnSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;
        
        for (int i = 0; i < enemies.Length; ++i)
        {
            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemies[i];
                break;
            }
        }

        if (nearestEnemy != null && shortestDistance <= _range)
        {
            _target = nearestEnemy.transform;
        }
        else
        {
            _target = null;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_target == null)
            return;

        TargetLookOn();
    }

    private void TargetLookOn()
    {
        var dir = _target.position - transform.position;
        var lookRotation = Quaternion.LookRotation(dir);
        var rotation = Quaternion.Lerp(_partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        _partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
