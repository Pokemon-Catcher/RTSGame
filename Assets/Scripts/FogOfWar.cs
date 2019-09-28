using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField]
    private RTSPlayer player;
    private Texture2D vision;
    private Texture2D exploration;
    [SerializeField]
    private Projector projector;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        vision = player.minimap.visibilityMap;
        exploration = player.minimap.exploredMap;
        projector.material.SetTexture("_visibleArea", vision);
        projector.material.SetTexture("_exploredArea", exploration);
    }

}
