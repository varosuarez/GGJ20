using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uploadable : MonoBehaviour
{
    [SerializeField]
    private bool _IsUploadable;

    private bool _IsUploaded;

    public bool IsUploadable
    {
        get { return _IsUploadable; }
        set { _IsUploadable = value; }
    }
    public bool IsUploaded
    {
        get { return _IsUploaded; }
        set { _IsUploaded = value; }
    }

}
