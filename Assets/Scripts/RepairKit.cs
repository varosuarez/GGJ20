using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : MonoBehaviour
{

    /// <summary>
    /// Rotation Speed
    /// </summary>
    /// 
    public float rotationSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    void OnTriggerEnter(Collider other)
    {
       //To do...
    }




}
