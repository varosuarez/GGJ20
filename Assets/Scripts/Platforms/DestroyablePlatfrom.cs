using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HCF;

public class DestroyablePlatfrom : MonoBehaviour
{
    public float timeStart = 2.0f;
    private float timeLeftToDestroy;
    private float timeLeftToEnable;
    public float sakeSpeed = 200f;
    bool isPlayerColliding = false;
    Vector3 startingPos, endPos;

    void Awake()
    {
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
        endPos.x = transform.position.x;
        endPos.y = transform.position.y;
        timeLeftToDestroy = timeStart;
        timeLeftToEnable = timeStart;
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

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            isPlayerColliding = true;
            timeLeftToDestroy -= Time.deltaTime;
            if(timeLeftToDestroy <= 0)
            {
               transform.GetChild(0).gameObject.SetActive(false);
               GetComponent<BoxCollider2D>().enabled = false;
               timeLeftToDestroy = timeStart;
                this.RunAfter(timeStart, () =>
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    GetComponent<BoxCollider2D>().enabled = true;
                    isPlayerColliding = false;
                }
                );
            }
        }
    }

    /*void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            timeLeftToEnable -= Time.deltaTime;
            if (timeLeftToEnable <= 0)
            {
                this.gameObject.SetActive(true);
                timeLeftToEnable = timeStart;
            }
            isPlayerColliding = false;
        }
    }*/


}
