using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFloor : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.collider.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.collider.transform.SetParent(null);
        }
    }
}
