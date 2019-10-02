using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;


public class UnitSelection : MonoBehaviour
{

    private float mouseX;
    private float mouseY;
    private bool isSelecting;
    private Vector3 startMousePoint;
    private Vector3 StartSelectingPoint;
    private Vector3 EndSelectingPoint;
    private float selectionHeight;
    private float selectionWidth;

    private Ray ray;
    private RaycastHit hit;

    [SerializeField]
    private Texture texture;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        isSelecting = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            startMousePoint = Input.mousePosition;

            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                StartSelectingPoint = hit.point;
            }
        }

        mouseX = Input.mousePosition.x;
        mouseY = Screen.height - Input.mousePosition.y;

        selectionWidth = startMousePoint.x - mouseX;
        selectionHeight = Input.mousePosition.y - startMousePoint.y;

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            DeselectUnits();
            if (startMousePoint == Input.mousePosition)
            {
                OneSelect();

            }
            else
            {
                MultiSelect();
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            GUI.DrawTexture(new Rect(mouseX, mouseY, selectionWidth, selectionHeight), texture);
        }
    }

    private void MultiSelect()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            EndSelectingPoint = hit.point;
        }
        FindUnitsInSelection();
    }

    private void FindUnitsInSelection()
    {

        MultiSelectEvent ev = new MultiSelectEvent() { firstCorner = StartSelectingPoint, secondCorner = EndSelectingPoint };
        EventAggregator.Publish<MultiSelectEvent>(ev);
    }

    private void DeselectUnits()
    {
        DeselectEvent ev = new DeselectEvent();
        EventAggregator.Publish(ev);
    }

    private void OneSelect()
    {
        if (hit.point != Vector3.zero && hit.collider != null && hit.collider.gameObject.tag == "RTSObject")
        {
            SelectEvent ev = new SelectEvent { hit = this.hit, Rts = hit.collider.GetComponent<RTSObject>() };
            EventAggregator.Publish<SelectEvent>(ev);
        }
    }
}
