using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySpells;

public class Hero : Unit
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        PassiveCustomAbility passiveAbility = new PassiveCustomAbility("passive ability", null);
        AbililtyComponent component = new AbilityFloatComponent("float", AbilityTools.ComponentTargets.Armor);
        passiveAbility.AddComponent(component);
        AbililtyComponent component2 = new AbilityIntComponent("int", AbilityTools.ComponentTargets.Health);
        passiveAbility.AddComponent(component2);
        AbililtyComponent component3 = AbilityTools.GetInstanceType(AbilityTools.ComponentTargets.Special, "special");
        passiveAbility.AddComponent(component3);

        foreach(AbililtyComponent abililtyComponent in passiveAbility.Components)
        {
            Debug.Log(abililtyComponent.name + " " + abililtyComponent.ComponentTargets);
        }
    }

    // Update is called once per frame
    protected override void Update()
    { 

        base.Update();
    }
     
    public override Dictionary<string, object> GetInfo()
    {
        //other props
            
        return base.GetInfo();
    }
}
