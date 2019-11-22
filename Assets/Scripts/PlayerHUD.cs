using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class PlayerHUD : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private GameObject unitHUD;

    [SerializeField]
    private GameObject button;

    [SerializeField]
    private Transform canvas;

    //stuff
    private Unit currentUnit;
    private GameObject[] transforms;

    [SerializeField]
    private List<GameObject> buttons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DrawIndividualHUD(Unit unit)
    {
        currentUnit = unit;
        unitHUD.SetActive(true);
        ConstructButtons();
    }

    public void ClearIndividualHUD()
    {
        foreach (GameObject item in buttons)
        {
            Destroy(item);
        }

        buttons.Clear();
        unitHUD.SetActive(false);
    }

    void ConstructButtons()
    {
        transforms = new GameObject[currentUnit.attributes.abilities.Count];
        for (int i = 0; i < currentUnit.attributes.abilities.Count; ++i)
        {
            transforms[i] = Instantiate(button, Vector3.zero, Quaternion.identity) as GameObject;
            transforms[i].transform.SetParent(canvas);
            //button.GetComponent<IndexOfSpellButton>().SetAbility(currentUnit.attributes.abilities[i].ID);
            //button.GetComponent<IndexOfSpellButton>().SetFunc(currentUnit.attributes.abilities[i].needOrder);
            buttons.Add(transforms[i]);
        }
    }
    
    void DrawButtons()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
