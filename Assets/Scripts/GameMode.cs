using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    public static GameMode instance = null;

    [SerializeField]
    private List<Building> buildings = new List<Building>();
    public List<Building> Buildings
    {
        get { return buildings; }
    }

    [SerializeField]
    private List<Unit> units = new List<Unit>();
    public List<Unit> Units
    {
        get { return units; }
    }

    [SerializeField]
    private List<RTSObject> rtsObjects = new List<RTSObject>();
    public List<RTSObject> RTSObjects
    {
        get { return rtsObjects; }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);
    }
}
