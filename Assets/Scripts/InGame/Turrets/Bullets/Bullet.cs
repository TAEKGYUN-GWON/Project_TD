using UnityEngine;

public class Bullet : ObjectBase
{

    private Transform _target;

    public float _speed = 70f;
    public GameObject impactEffectPrefab;

    //무엇이 더 효율적인가..
    /*private ParticleSystem _impact;

    protected override void Awake()
    {
        base.Awake();
        _impact =  Instantiate(impactEffectPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        _impact.Stop();
    }*/

    public void Seek(Transform target)
    {
        _target = target;
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

        /*var impactTrans = _impact.transform;
        impactTrans.position = transform.position;
        impactTrans.rotation = transform.rotation;
        _impact.Play();*/

        var impact = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
        
        Destroy(impact, 2f);
        
        Despawn();
    }
    
}
