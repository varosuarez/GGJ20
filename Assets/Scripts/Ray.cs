using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{

    public bool red = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DinamicPlayer>() == null)
        {
            return;
        }

        if (red)
        {

            if (other.gameObject.GetComponent<DinamicPlayer>().IsInPhase()) {
                //SiJNoC 
                transform.Find("Collider").gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
            }
            else
            {
                //NoJNoC              
                transform.Find("Collider").gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        else
        {
            if (other.gameObject.GetComponent<DinamicPlayer>().IsInPhase()) {
                //NoJSiC
                transform.Find("Collider").gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                //SiJSiC
                transform.Find("Collider").gameObject.layer = LayerMask.NameToLayer("IgnorePlayerAndBox");
            }
        }

    }

}

