using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catchable : MonoBehaviour
{
    [SerializeField]
    private bool _IsCatchable = true;

    public bool IsCatchable
    {
        get { return _IsCatchable; }
        set { _IsCatchable = value; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DinamicPlayer>() != null)  //Si no se comprueba apareceran demasiado warnings
            other.SendMessage("EnableCatch", transform.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DinamicPlayer>() != null)  //Si no se comprueba apareceran demasiado warnings
            other.SendMessage("DisableCatch");
    }


}
