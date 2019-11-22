using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCrafter : MonoBehaviour
{
    private GameObject button;  
    private List<Ability> abilities;

    [SerializeField]
    private GameObject[] gameObjects;

    private Transform canvas;

    public ButtonCrafter(List<Ability> abilities, GameObject button, Transform canvas)
    {
        this.abilities = abilities;
        this.button = button;
        this.canvas = canvas;
    }

    void Start()
    {
        //EventAggregation.EventAggregator.Subscribe<EventAggregation.DeselectEvent>(OnDeselect);
    }

    //private void OnDeselect(EventAggregation.IEventBase eventBase)
    //{
    //    EventAggregation.DeselectEvent ev = eventBase as EventAggregation.DeselectEvent;
        
    //    if(!(ev is null))
    //    {
    //        foreach (GameObject item in gameObjects)
    //        {
    //            Destroy(item);
    //        }

    //        gameObjects = null;
    //    }
    //}

    public GameObject[] MakeButtons()
    {
        gameObjects = new GameObject[abilities.Count]; 

        for(int i = 0; i < gameObjects.Length; ++i)
        {
            gameObjects[i] = Instantiate(button, Vector2.zero, Quaternion.identity) as GameObject;
            gameObjects[i].transform.SetParent(canvas, false);
            //gameObjects[i].GetComponent<IndexOfSpellButton>().SetAbility(abilities[i]);
        }

        return gameObjects;
    }
}

