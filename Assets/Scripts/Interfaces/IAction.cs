using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    void StartAction();
    void UpdateAction();
    void FinishAction();
    bool IsFinished();
}
