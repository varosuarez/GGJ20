using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Transform startpos, endpos;
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        startpos = this.transform;
    
    }

    internal void Activate()
    {
        Vector3 aux;
        endpos = startpos;
        aux = startpos.position;
        aux.y += 4;
        endpos.position = aux;
        transform.position = endpos.position;
    }

    internal void DeActivate()
    {
        transform.position = startpos.position;
    }
}
