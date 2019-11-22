using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexOfSpellButton : MonoBehaviour
{
    [SerializeField]
    private int ability;

    public void SetAbility(int ability)
    {
        this.ability = ability;
    }

    public void SetFunc(bool needOrder)
    {
        if (needOrder)
        {
            GetComponent<Button>().onClick.AddListener(delegate { transform.Find("Player").GetComponent<RTSPlayer>().MakeOrderWithParams(ability); });
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(delegate { transform.Find("Player").GetComponent<RTSPlayer>().MakeOrder(ability); });
        }
    }
}
    