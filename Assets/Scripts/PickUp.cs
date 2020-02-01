using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{

    private Transform taker;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.parent = taker;
            //this.transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<DinamicPlayer>().carryingPos.position;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            
            transform.parent = null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
        {
            Debug.Log("E");
            // taker = col.GetComponent<DinamicPlayer>().carryingPos;
            Destroy(GetComponent<Rigidbody2D>());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
        {
            Debug.Log("Ew");
            taker = null;
            gameObject.AddComponent<Rigidbody2D>();
        }

    }
}