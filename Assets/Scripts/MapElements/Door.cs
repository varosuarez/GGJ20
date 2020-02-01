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

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Activate()
    {
        Vector3 aux;
        endpos = startpos;
        aux = startpos.position;
        aux.y += 4;
        endpos.position = aux;
        transform.position = Vector3.Lerp(startpos.position, endpos.position, speed * Time.deltaTime);
    }

    internal void DeActivate()
    {
        Vector3 aux;
        endpos = startpos;
        aux = startpos.position;
        aux.y -= 4;
        endpos.position = aux;
        transform.position = Vector3.Lerp(endpos.position, startpos.position, speed * Time.deltaTime);
    }
}
