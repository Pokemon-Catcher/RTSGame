using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Ability : MonoBehaviour, IExecutable
{
    public abstract void Execute(params object[] objects);

    public void AtmrMove(NavMeshAgent nav, Vector3 dir)
    {
        nav.SetDestination(dir);
    }
}
