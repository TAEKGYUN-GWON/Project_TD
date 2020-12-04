
using System.Collections.Generic;
using UnityEngine;

public enum E_ENEMY_TYPE : int
{
    NONE = 0,
    BASIC = 1,
    SPEED = 2
}

public class Enemy : ObjectBase
{
    public float speed = 0f;

    private E_ENEMY_TYPE eType = E_ENEMY_TYPE.NONE;
    private Transform target;
    private int wayPointIdx = 0;
    private List<Transform> wayPoints;

    public ObjectPool<Enemy> Pool
    {
        set => pool = value;
    }

    ObjectPool<Enemy> pool = new ObjectPool<Enemy>();
    void Start()
    {
        wayPoints = WayPonts.points;
        target = wayPoints[wayPointIdx];
    }

    void FixedUpdate()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            NextWayPoint();
        }
    }

    void NextWayPoint()
    {
        if (wayPointIdx >= wayPoints.Count - 1)
        {
            wayPointIdx = 0;
            target = wayPoints[wayPointIdx];
            Despawn();
            return;
        }

        wayPointIdx++;
        target = wayPoints[wayPointIdx];
    }


}
