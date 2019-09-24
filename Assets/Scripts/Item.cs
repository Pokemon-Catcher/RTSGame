using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : RTSObject, IDestructable
{
    public int Health { get; set; }

    public override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    public void TakeDamage(int count)
    {

    }
}
