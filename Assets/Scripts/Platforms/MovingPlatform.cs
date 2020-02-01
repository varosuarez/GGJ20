using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform waypoint1, waypoint2;
    Transform currentPoint;

    private void Awake()
    {

    }
    private void Update()
    {
        transform.position = Vector3.Lerp(waypoint1.position, waypoint2.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2);
    }
}
