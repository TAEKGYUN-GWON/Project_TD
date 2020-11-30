
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wayPointIdx = 0;
    private List<Transform> wayPoints;
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
