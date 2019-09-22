using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class TestPublisher : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseLeftClickEvent ev = new MouseLeftClickEvent() { Point = cam.ScreenPointToRay(Input.mousePosition)};

            EventAggregator.Publish<MouseLeftClickEvent>(ev);
        }
    }
}
