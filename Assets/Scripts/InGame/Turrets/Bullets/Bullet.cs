using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectBase
{

    private Transform _target;

    public float _speed = 70f;
    public GameObject impactEffectPrefab;
    private ParticleSystem impact;
    public void Seek(Transform target)
    {
        _target = target;
    }

    protected override void Awake()
    {
        base.Awake();
        
        impact = Instantiate(impactEffectPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        
        impact.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = _speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        var enemy = _target.GetComponent<Enemy>();
        enemy.Despawn();
        var impactTransform = impact.transform;
        impactTransform.position = transform.position;
        impactTransform.rotation = transform.rotation;
        
        impact.Play();
        Despawn();
    }
    
}
