using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePlatfrom : MonoBehaviour
{
    public float timeToDestroy = 2.0f;
    public float sakeSpeed = 200f;
    bool isPlayerColliding = false;
    Vector3 startingPos, endPos;

    void Awake()
    {
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
        endPos.x = transform.position.x;
        endPos.y = transform.position.y;
    }


    private void Update()
    {
        if (isPlayerColliding)
        {
            float aux = startingPos.x + Random.insideUnitSphere.x * 0.05f;
            endPos.x = aux;
            transform.localPosition = endPos;
        }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            isPlayerColliding = true;
            Destroy(gameObject, timeToDestroy);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            timeToDestroy = 1;
            isPlayerColliding = false;
        }
    }
}
