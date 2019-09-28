using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : MonoBehaviour
{
    public int id = 0;
    public string playerName="Player";
    public int gold=0;
    public int wood=0;
    public int food=0;
    public Camera playerCamera;
    public Minimap minimap;
    public List<Unit> units = new List<Unit>();

    private void Awake()
    {
    }
}
