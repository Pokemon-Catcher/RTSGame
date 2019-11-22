using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IGMove : ParametricAbility
{
    float time;

    void Start()
    {
        base.buttonObject = Instantiate(base.buttonObject, Vector3.zero, Quaternion.identity) as GameObject;
        buttonObject.transform.SetParent(pos);
        buttonObject.transform.position = Vector3.zero;
        base.button = buttonObject.GetComponent<Button>();
    }

    public override void FinishAction()
    {
        Debug.Log("done");
    }

    public override bool IsFinished()
    {
        return time > 4f;
    }

    public override void StartAction()
    {
        //Debug.Log(abilityParams.unit.transform.position + " dasd");
        Debug.Log("de" + abilityParams.unit.transform.position);
    }

    public override void UpdateAction()
    {
        time += Time.deltaTime;
    }
}
