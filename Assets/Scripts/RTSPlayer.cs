using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;


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
    private void Awake()
    {
        EventAggregator.Subscribe<SelectEvent>(OnSelect);
        EventAggregator.Subscribe<MultiSelectEvent>(OnMultiSelect);
        EventAggregator.Subscribe<DeselectEvent>(OnDeselect);
    }

    private void OnSelect(IEventBase eventBase)
    {
        SelectEvent ev = eventBase as SelectEvent;
        Unit unit = ev.hit.collider.GetComponent<Unit>();
        if (ev != null && units.Contains(unit))
        {
            selectedUnits.Add(unit);
            Debug.Log("dada");
        }
    }

    private void OnMultiSelect(IEventBase eventBase)
    {
        MultiSelectEvent ev = eventBase as MultiSelectEvent;

        Vector3 StartSelectingPoint = ev.firstCorner;
        Vector3 EndSelectingPoint = ev.secondCorner;

        //foreach (Unit unit in units)
        //{
        //    float x = unit.transform.position.x;
        //    float z = unit.transform.position.z;
        //    if (x > StartSelectingPoint.x && x < EndSelectingPoint.x || (x < StartSelectingPoint.x && x > EndSelectingPoint.x))
        //    {
        //        if (z > StartSelectingPoint.z && z < EndSelectingPoint.z || (z < StartSelectingPoint.z && z > EndSelectingPoint.z))
        //        {
        //            selectedUnits.Add(unit);
        //            Debug.Log("dada2");
        //        }
        //    }
        //}
        Vector3 scale = StartSelectingPoint - EndSelectingPoint;
        scale.x = Mathf.Abs(scale.x);
        scale.y = Mathf.Abs(scale.y);
        scale.z = Mathf.Abs(scale.z);

        Vector3 center = (StartSelectingPoint + EndSelectingPoint) * 0.5f; 
        Vector3 halfExtents = scale * 0.5f;
        RaycastHit[] hits = Physics.BoxCastAll(center, halfExtents, Vector3.up, Quaternion.identity, 100f, 1 << 10);

        foreach (RaycastHit hit in hits)
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if (units.Contains(unit))
                selectedUnits.Add(unit);
        }
    }

    private void OnDeselect(IEventBase eventBase)
    {
        DeselectEvent ev = eventBase as DeselectEvent;

        if (ev != null)
        {
            selectedUnits.Clear();
            
            Debug.Log("dada3");
        }
    }
}
