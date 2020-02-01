using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
    [SerializeField]
    private bool _IsAttachable;
    private bool _IsAttached;
    public bool IsAttachable
    {
        get { return _IsAttachable; }
        set { _IsAttachable = value; }
    }
    public bool IsAttached
    {
        get { return _IsAttached; }
        set { _IsAttached = value; }
    }
}
