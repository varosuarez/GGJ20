using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public List<Door> doors = new List<Door>();
    public  Sprite isPressed, isNotPressed;
    private bool doorActive = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isPressed)
            this.GetComponent<SpriteRenderer>().sprite = isNotPressed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Movable" && !doorActive)
        {
            foreach (Door d in doors)
            {
                d.Activate();
                this.GetComponent<SpriteRenderer>().sprite = isPressed;
                doorActive = true;
                Destroy(other.GetComponent<PickUp>());
            }
        }
    }
}
