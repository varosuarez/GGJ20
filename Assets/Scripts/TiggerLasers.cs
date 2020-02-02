using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiggerLasers : MonoBehaviour
{

    public GameObject laserRed1;
    public GameObject laserRed2;
    public GameObject laserBlue1;
    public GameObject laserBlue2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            laserRed1.SetActive(true);
            laserRed2.SetActive(true);
            laserBlue1.SetActive(true);
            laserBlue2.SetActive(true);

        }
    }
}
