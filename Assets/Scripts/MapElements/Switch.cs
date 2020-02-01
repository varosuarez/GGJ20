using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public List<Door> doors = new List<Door>();
    public Sprite isPressed, isNotPressed;
    // Start is called before the first frame update
    void Start()
    {
        if (isPressed && isNotPressed)
            this.GetComponent<SpriteRenderer>().sprite = isPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Movable")
        {
            foreach (Door d in doors)
            {
                d.Activate();
                this.GetComponent<SpriteRenderer>().sprite = isPressed;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Movable")
        {
            foreach (Door d in doors)
            {
                d.DeActivate();
                this.GetComponent<SpriteRenderer>().sprite = isNotPressed;
            }
        }
    }
}
