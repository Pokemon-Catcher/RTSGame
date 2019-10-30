using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGAttack : Ability
{
    public override void Execute(params object[] objects)
    {
        Debug.Log("Attack!");
    }
}
