using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TConstrBuild : MonoBehaviour
{
    public static _TConstrBuild instance = null;
    private GameObject building = null;
    private GameObject ghost = null;
    private bool locker = false;

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
        if (ghost != null)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 200.0f, 1 << 8))
            {
                ghost.transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0) && intersects == 0)
            {
                Instantiate(building, ghost.transform.position, Quaternion.identity);
                Destroy(ghost);
                locker = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(ghost);
                building = null;
                locker = false;
            }
        }
    }

    public void Construct(GameObject building)
    {
        if (!locker && building != null)
        {
            locker = true;
            this.building = building;
            GameObject ghost = this.building.GetComponent<Building>().GhostBuilding;
            this.building.GetComponent<Building>().SetMesh(this.building.GetComponent<MeshFilter>().sharedMesh);
            ghost.transform.rotation = this.building.transform.rotation;

            if (ghost != null)
                this.ghost = (GameObject)Instantiate(ghost);
        }
    }
}
