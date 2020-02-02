using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public List<Door> doors = new List<Door>();
    public  Sprite isPressed, isNotPressed;
    private bool doorActive = false;
    public AudioClip m_doorSound;

    // Start is called before the first frame update
    void Start()
    {

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

                GameObject.FindGameObjectWithTag("Player").GetComponent<DinamicPlayer>().SendMessage("DisableCatch");
                if (m_doorSound != null)
                {
                    AudioSource m_audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
                    m_audio.clip = m_doorSound;
                    m_audio.Play();
                }

            }
        }
    }
}
