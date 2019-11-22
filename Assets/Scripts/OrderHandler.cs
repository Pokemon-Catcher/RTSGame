using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderHandler : MonoBehaviour
{
    private Queue<IAction> abilities = new Queue<IAction>();
    private IAction currentAbility;
    private Unit unit;
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAbility == null)
        {
            if (abilities.Count > 0)
            {
                currentAbility = abilities.Dequeue();
                currentAbility.StartAction();
            }
        }
        else
        {
            currentAbility.UpdateAction();
            if (currentAbility.IsFinished())
            {
                currentAbility.FinishAction();
                currentAbility = null;
            }
        }
    }

    public void AddAction(int index)
    {
        //Ability ability = unit.attributes.abilities.FirstOrDefault(item => item.ID == index);
        //if (ability != null)
        //    abilities.Enqueue(ability);
        //else Debug.Log("vsetaki null");
    }
}
