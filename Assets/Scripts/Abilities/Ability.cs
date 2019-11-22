using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour, IAction
{

    [SerializeField]
    protected Transform pos;

    [SerializeField]
    protected GameObject buttonObject;
    protected Button button;

    public abstract void FinishAction();

    public abstract bool IsFinished();

    public abstract void StartAction();

    public abstract void UpdateAction();
    
}
