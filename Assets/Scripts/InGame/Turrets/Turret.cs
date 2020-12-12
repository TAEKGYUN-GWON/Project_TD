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

    
    [Header("Attributes")]
    public float _range = 15f;
    public float _fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    
    [SerializeField]
    private Transform _partToRotate;

    public float turnSpeed = 10f;
    
    ObjectBasePool _bulletPool;

    public Bullet bulletPrefab;
    public Transform firePoint;

    private void Awake()
    {
        init();
    }

    void init()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        
        _bulletPool = ObjectBasePool.CreateInstancePool(transform, bulletPrefab, 50, true, 50);
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

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / _fireRate;
        }

        fireCountdown -= Time.deltaTime;

    }

    private void TargetLookOn()
    {
        var dir = _target.position - transform.position;
        var lookRotation = Quaternion.LookRotation(dir);
        var rotation = Quaternion.Lerp(_partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        _partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        var bullet = _bulletPool.Spawn<Bullet>();
        var bulletTransform = bullet.gameObject.transform;
        bulletTransform.position = firePoint.position;
        bulletTransform.rotation = firePoint.rotation;
        bullet.gameObject.transform.SetParent(transform);

        if (bullet != null)
        {
            bullet.Seek(_target);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
