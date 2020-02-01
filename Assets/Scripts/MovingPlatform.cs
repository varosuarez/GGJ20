using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    bool isMovingRight;
    public Transform waypoint1, waypoint2;
    Transform currentPoint;

    private void Awake()
    {
        if(!waypoint1 || !waypoint2)
        {
            Debug.LogError("Waypoint is null!");
        }
    }
    private void Update()
    {
       /* transform.position = Vector2.MoveTowards(transform.position, waypoint1.transform.position, speed * Time.deltaTime);

        transform.position = Vector2.MoveTowards(transform.position, waypoint2.transform.position, speed * Time.deltaTime);*/
    }
}
