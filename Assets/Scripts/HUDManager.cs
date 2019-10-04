using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventAggregation;

public class HUDManager : MonoBehaviour
{
    [Header("General HUD")]
    [SerializeField]
    private GameObject GeneralHUD;

    [SerializeField]
    private GameObject UnitHUD;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private DayCycle dayCycle;

    [Header("Unit HUD")]
    [SerializeField]
    private Text unitName;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text health;

    private bool startHUDUpdate = false;
    private bool startUnitUpdate = false;
    private Dictionary<string, object> info;

    void Awake()
    {
      EventAggregator.Subscribe<SelectEvent>(DrawUnitHUD);
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawGeneralHUD();
    }

    void DrawGeneralHUD()
    {
        startHUDUpdate = true;
        GeneralHUD.SetActive(true);   
    }

    void DrawUnitHUD(IEventBase rts)
    {
        SelectEvent ev = rts as SelectEvent;
        //icon = ev.Rts.Icon;
        ev.Rts.Info.Clear();
        info = ev.Rts.GetInfo();
        unitName.text = (string)info["name"];

        startUnitUpdate = true;
        UnitHUD.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (startHUDUpdate)
        {
            timeText.text = dayCycle.time.hours + ":" + dayCycle.time.minutes;
        }

        if (startUnitUpdate)
        {
            health.text = (int)info["health"] + "/" + (int)info["maxHealth"];
            //if (info.ContainsKey("xp"))
            //    health.text = ((int)info["xp"]).ToString();
        }
    }
}
