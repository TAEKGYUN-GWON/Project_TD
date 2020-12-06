
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType : int
{
    None = 0,
    Basic = 1,
    Speed = 2
}

public class Enemy : ObjectBase
{
    public float speed = 0f;

    private EEnemyType _eType = EEnemyType.None;
    private Transform _target;
    private int _wayPointIdx = 0;
    private List<Transform> _wayPoints;

    public ObjectPool<Enemy> Pool
    {
        set => _pool = value;
    }

    ObjectPool<Enemy> _pool = new ObjectPool<Enemy>();
    void Start()
    {
        _wayPoints = WayPonts.Points;
        _target = _wayPoints[_wayPointIdx];
    }

    void FixedUpdate()
    {
        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, _target.position) <= 0.4f)
        {
            NextWayPoint();
        }
    }

    void NextWayPoint()
    {
        if (_wayPointIdx >= _wayPoints.Count - 1)
        {
            _wayPointIdx = 0;
            _target = _wayPoints[_wayPointIdx];
            Despawn();
            return;
        }

        _wayPointIdx++;
        _target = _wayPoints[_wayPointIdx];
    }


}
