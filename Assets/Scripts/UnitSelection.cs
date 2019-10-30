using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;
using UnityEngine.EventSystems;

internal class UnitSelection : MonoBehaviour
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

    [SerializeField]
    private EventSystem evSystem;

    // Use this for initialization
    void Start()
    {
        Debug.Log(evSystem == null);
        isSelecting = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SelectUnit();
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            GUI.DrawTexture(new Rect(mouseX, mouseY, selectionWidth, selectionHeight), texture);
        }
    }

    private void SelectUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (evSystem.IsPointerOverGameObject())
            {
                //stuff on the hud 
            }
            else
            {
                isSelecting = true;
                startMousePoint = Input.mousePosition;

                ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    StartSelectingPoint = hit.point;
                }
            }
        }

        mouseX = Input.mousePosition.x;
        mouseY = Screen.height - Input.mousePosition.y;

        selectionWidth = startMousePoint.x - mouseX;
        selectionHeight = Input.mousePosition.y - startMousePoint.y;

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;

            if (evSystem.IsPointerOverGameObject())
            {
                //stuff on the hud 
            }
            else
            {
                DeselectUnits();
                MultiSelect();
            }
        }
    }

    private void MultiSelect()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            EndSelectingPoint = hit.point;
        }

        WorldSelectEvent ev = new WorldSelectEvent() { firstCorner = StartSelectingPoint, secondCorner = EndSelectingPoint };
        EventAggregator.Publish<WorldSelectEvent>(ev);
    }

    private void DeselectUnits()
    {
        DeselectEvent ev = new DeselectEvent();
        EventAggregator.Publish(ev);
    }
        //SelectEvent ev = new SelectEvent { hit = this.hit, Attributes = XmlTools.ParseXmlToObject("Assets/UnitAttirbutes/" + hit.collider.GetComponent<RTSObject>().Name + ".txt") };
}
