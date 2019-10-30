using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventAggregation;
using System;

public class HUDManager : MonoBehaviour
{
    [Header("General HUD")]
    [SerializeField]
    private GameObject GeneralHUD;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private DayCycle dayCycle;
    private bool startUpdate;

    public static HUDManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);
    }

    void Start()
    {
        DrawGeneralHUD();
    }

    void DrawGeneralHUD()
    {
        GeneralHUD.SetActive(true);
        startUpdate = true;
    }

    public void UpdateGeneralHUD()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (startUpdate)
        { 
            timeText.text = dayCycle.time.hours + ":" + dayCycle.time.minutes;
        }
    }
}
