using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class RTSObject : MonoBehaviour
{
    [SerializeField]
    public Collider visibilityBlocker;

    [SerializeField]
    protected string Name;

    [SerializeField]
    protected bool selected;

    public virtual void Awake()
    {
        //GameMode.getInstance().Units.Add(gameObject);
        Debug.Log(GameMode.getInstance() == null);
        EventAggregator.Subscribe<SelectEvent>(OnSelect);
        EventAggregator.Subscribe<MultiSelectEvent>(OnMultiSelect);
        EventAggregator.Subscribe<DeselectEvent>(OnDeselect);
    }

    protected void OnMultiSelect(IEventBase eventBase)
    {
        MultiSelectEvent selectEvent = eventBase as MultiSelectEvent;

        float x = transform.position.x;
        float z = transform.position.z;

        Vector3 StartSelectingPoint = selectEvent.firstCorner;
        Vector3 EndSelectingPoint = selectEvent.secondCorner;

        if (x > StartSelectingPoint.x && x < EndSelectingPoint.x || (x < StartSelectingPoint.x && x > EndSelectingPoint.x))
        {
            if (z > StartSelectingPoint.z && z < EndSelectingPoint.z || (z < StartSelectingPoint.z && z > EndSelectingPoint.z))
            {
                selected = true;
                Debug.Log("I'm multiselected: " + gameObject.name);
            }
        }
    }

    protected void OnSelect(IEventBase eventBase)
    {
        SelectEvent select = eventBase as SelectEvent;

        if(select.hit.collider.gameObject == gameObject)
        {
            selected = true;
            Debug.Log("I'm selected: " + gameObject.name);
        }
    }

    protected void OnDeselect(IEventBase eventBase)
    {
        DeselectEvent ev = eventBase as DeselectEvent;

        if(ev != null)
        {
            selected = false;
            Debug.Log("I'm deselected: " + gameObject.name);
        }
    }

    public virtual void Start()
    { 
    }
}
