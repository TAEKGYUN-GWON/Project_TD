
using System.Collections.Generic;
using UnityEngine;

public enum E_ENEMY_TYPE : int
{
    NONE = 0,
    BASIC = 1,
    SPEED = 2
}

public class Enemy : MonoBehaviour
{
    private float speed = 0f;

    private E_ENEMY_TYPE eType = E_ENEMY_TYPE.NONE;
    private Transform target;
    private int wayPointIdx = 0;
    private List<Transform> wayPoints;

    public E_ENEMY_TYPE Type { get => eType; set => Type = CastTo<E_ENEMY_TYPE>.From(value); }

    void Start()
    {
        wayPoints = WayPonts.points;
        target = wayPoints[wayPointIdx];
    }

    void Update()
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
            gameObject.SetActive(false);
            return;
        }

        wayPointIdx++;
        target = wayPoints[wayPointIdx];
    }


}
