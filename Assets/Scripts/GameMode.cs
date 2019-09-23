using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private static GameMode instance;

    [SerializeField]
    private List<GameObject> units = new List<GameObject>();
    public List<GameObject> Units { get; }

    private GameMode()
    {

    }

    public static GameMode getInstance()
    {
        if (instance == null)
            instance = new GameMode();
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
