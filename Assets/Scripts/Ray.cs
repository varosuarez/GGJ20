using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{

    public GameManager.Phase m_RayColor = GameManager.Phase.RED_RIGHT;
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

        if (m_RayColor == other.gameObject.GetComponent<DinamicPlayer>().GetPhase())
        {
            m_Collider.enabled = false;
        }
        else {
            m_Collider.enabled = true;
        }
    }

}

