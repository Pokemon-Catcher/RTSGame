using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexOfSpellButton : MonoBehaviour
{
    [SerializeField]
    private Ability ability;

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        Debug.Log("my spell is: " + this.ability == null);
    }

    public void SetAbilityInfo()
    {
        OrderHandler.HandleAbility(this.ability);
    }
}
