using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    private List<Unit> buyableUnits { get; }
    private List<Unit> trainableUnits { get; }
    private float trainSpeed;

    [SerializeField]
    private GameObject ghostBuilding = null;
    public GameObject GhostBuilding
    {
        get { return ghostBuilding; }
    }

    public void SetMesh(Mesh mesh)
    {
        ghostBuilding.GetComponent<MeshFilter>().mesh = mesh;
    }

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

    public override Dictionary<string, object> GetInfo()
    {

        return base.GetInfo();
    }

    protected override void Update()
    {
        base.Update();

    }
}
