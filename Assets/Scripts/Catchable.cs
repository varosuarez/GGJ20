using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catchable : MonoBehaviour
{
    [SerializeField]
    private bool _IsCatchable;

    private bool _IsCatched;

    public bool IsCatchable
    {
        get { return _IsCatchable; }
        set { _IsCatchable = value; }
    }
    public bool IsCatched
    {
        get { return _IsCatched; }
        set { _IsCatched = value; }
    }

}
