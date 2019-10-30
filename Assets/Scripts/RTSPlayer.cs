using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;
using System;

public class RTSPlayer : MonoBehaviour
{
    public int id = 0;
    public string playerName="Player";
    public int gold=0;
    public int wood=0;
    public int food=0;
    public Camera playerCamera;
    public Minimap minimap;
    public List<Unit> units = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();

    //hud and ability stuffs
    private Dictionary<string, RTSObject> squadTypes = new Dictionary<string, RTSObject>();

    [SerializeField]
    private PlayerHUD playerHUD;
    //private OrderHandler handler;

    private void Awake()
    {
        playerHUD = GetComponent<PlayerHUD>();
        EventAggregator.Subscribe<WorldSelectEvent>(OnWorldSelect);
        EventAggregator.Subscribe<DeselectEvent>(OnDeselect);
    }

    private void OnWorldSelect(IEventBase eventBase)
    {

        WorldSelectEvent ev = eventBase as WorldSelectEvent;

        Vector3 StartSelectingPoint = ev.firstCorner;
        Vector3 EndSelectingPoint = ev.secondCorner;

        if (Mathf.Approximately(StartSelectingPoint.magnitude, EndSelectingPoint.magnitude))
            StartSelectingPoint = new Vector3(StartSelectingPoint.x + 1f, StartSelectingPoint.y, StartSelectingPoint.z + 1f);            

        Vector3 scale = StartSelectingPoint - EndSelectingPoint;
        scale.x = Mathf.Abs(scale.x);
        scale.y = Mathf.Abs(scale.y);
        scale.z = Mathf.Abs(scale.z);

        Vector3 center = (StartSelectingPoint + EndSelectingPoint) * 0.5f; 
        Vector3 halfExtents = scale * 0.5f;
        RaycastHit[] hits = Physics.BoxCastAll(center, halfExtents, Vector3.up, Quaternion.identity, 100f, 1 << 10);

        squadTypes.Clear();

        foreach (RaycastHit hit in hits)
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if (!(unit is null) && units.Contains(unit))
            {
                selectedUnits.Add(unit);
                    
                if (!squadTypes.ContainsKey(unit.Name))
                    squadTypes.Add(unit.Name, unit);
            }
        }

        //drawer.DrawHUDfromType(squadTypes["Swordsman"] as Unit, button, canvas); // squadTypes[Name] <--- Name or ID will be taken from button

        if (squadTypes.Count > 0)
        {
            playerHUD.DrawIndividualHUD(squadTypes["Swordsman"] as Unit);
        }
    }

    private void OnDeselect(IEventBase eventBase)
    {
        DeselectEvent ev = eventBase as DeselectEvent;

        if (!(ev is null))
        {
            selectedUnits.Clear();
            playerHUD.ClearIndividualHUD();
        }
    }
}
