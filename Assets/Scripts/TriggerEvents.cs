using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "RTSObject")
        {
            _TConstrBuild.instance.intersects++;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "RTSObject")
        {
            _TConstrBuild.instance.intersects--;
        }
    }
}
