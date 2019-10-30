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
        Debug.Log(currentUnit.attributes.abilities.Count);
        ConstructButtons(currentUnit.attributes.abilities);
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

    void ConstructButtons(List<Ability> abilities)
    {
        transforms = new GameObject[abilities.Count];
        for (int i = 0; i < abilities.Count; ++i)
        {
            transforms[i] = Instantiate(button, Vector3.zero, Quaternion.identity) as GameObject;
            transforms[i].transform.SetParent(canvas);
            button.GetComponent<IndexOfSpellButton>().SetAbility(abilities[i]);
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
