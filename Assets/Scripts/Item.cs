using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : RTSObject, IDestructable
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    protected override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

    }

    public void TakeDamage(int count)
    {

    }
}
