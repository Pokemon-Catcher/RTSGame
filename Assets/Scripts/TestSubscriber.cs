using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class TestSubscriber : MonoBehaviour
{
    private RaycastHit hit;

    void Awake()
    {
        EventAggregator.Subscribe<MouseLeftClickEvent>(OnSimpleEvent);
    }

    private void OnSimpleEvent(IEventBase eventBase)
    {
        MouseLeftClickEvent ev = eventBase as MouseLeftClickEvent;

        if(ev != null)
        {
            if (Physics.Raycast(ev.Point, out hit))
            {
                if(hit.collider.tag == "Unit")
                {
                    Debug.Log("I'm " + hit.collider.name);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
