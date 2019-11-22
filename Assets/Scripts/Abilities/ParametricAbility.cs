using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParametricAbility : Ability
{
    public Parametrs abilityParams = new Parametrs();
    public AbilityParamTypes paramTypes;
}

public struct Parametrs
{
    public Unit unit;
    public Vector3 point;
    public float radius;
}

public enum AbilityParamTypes
{
    unit,
    point,
    pointPlusRadius,
    unitPlusRadius,
}

