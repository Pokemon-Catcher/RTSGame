using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TConstrBuild : MonoBehaviour
{
    public static _TConstrBuild instance = null;
    private GameObject building = null;

    //raycasting
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    [SerializeField]
    public int intersects = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (building != null)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 200.0f, 1 << 8))
            {
                building.transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0) && intersects == 0)
            {
                building.tag = "RTSObject";
                building = null;
                ChangeTriggerState(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(building);
                ChangeTriggerState(false);
            }
        }
    }

    void ChangeTriggerState(bool state)
    {
        for (int i = 0; i < GameMode.instance.RTSObjects.Count; ++i)
        { 
            GameMode.instance.RTSObjects[i].gameObject.GetComponent<Collider>().isTrigger = state;
        }
    }

    public void Construct(GameObject building)
    {
        ChangeTriggerState(true);

        if (building != null)
            this.building = (GameObject)Instantiate(building);

        this.building.tag = "Constructing";
    }
}
