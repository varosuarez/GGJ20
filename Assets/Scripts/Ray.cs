﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{

    public bool red = false;
    private Collider2D m_Collider = null;

    // Start is called before the first frame update
    void Start()
    {
       Collider2D[] a = GetComponents<Collider2D>();
        if (a[0].isTrigger)
        {
            m_Collider = a[1];
        }
        else
        {
            m_Collider = a[0];
        }
      
    }

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
                
                gameObject.layer = LayerMask.NameToLayer("IgnorePlayerNotBox");
            }
            else
            {
                //NoJNoC              
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        else
        {
            if (other.gameObject.GetComponent<DinamicPlayer>().IsInPhase()) {
                //NoJSiC
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                //SiJSiC
                gameObject.layer = LayerMask.NameToLayer("IgnorePlayerAndBox");
            }
        }

    }

}

