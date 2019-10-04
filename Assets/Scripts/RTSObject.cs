using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;
using UnityEngine.UI;

public class RTSObject : MonoBehaviour
{
    [SerializeField]
    public Collider visibilityBlocker;

    [SerializeField]
    protected string rtsName;
    public string Name
    {
        get { return rtsName; }
    }

    protected Image rtsIcon;
    public Image Icon
    {
        get { return rtsIcon; }
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        GameMode.instance.RTSObjects.Add(this);
    }

    protected virtual void Update()
    {

    }
}
