using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    private List<Unit> buyableUnits { get; }
    private List<Unit> trainableUnits { get; }
    private float trainSpeed;

    protected override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GameMode.instance.Buildings.Add(this);
    }

    protected override void Update()
    {
        base.Update();

    }
}
