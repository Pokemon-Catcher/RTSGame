using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TConstrBuild : MonoBehaviour
{
    public static _TConstrBuild instance = null;
    private GameObject building = null;
    private GameObject ghost = null;
    private bool locker = false;

    [SerializeField]
    private float marginOfDifference = 0f;

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

                //fix this 
                //ghost.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            }

            if (Input.GetMouseButtonDown(0) && CheckBuildPossibility(ghost.GetComponent<BoxCollider>()))
            {
                Instantiate(building, ghost.transform.position, ghost.transform.rotation);
                Destroy(ghost);
                locker = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (intersects > 0)
                    intersects = 0;

                Destroy(ghost);
                building = null;
                locker = false;
            }
            UpdateBuildPossibilityColor(CheckBuildPossibility(ghost.GetComponent<BoxCollider>()), ghost);
        }
    }

    void UpdateBuildPossibilityColor(bool state, GameObject ghost)
    {
        Material mat = ghost.GetComponent<MeshRenderer>().material;

        if (state)
            mat.color = Color.green;

        else
            mat.color = Color.red;

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.5f);
    }

    bool CheckBuildPossibility(BoxCollider col)
    {
        Vector3[] corners = new Vector3[12];

        //bottom corners of box collider
        corners[0] = col.center + new Vector3(col.size.x, -col.size.y + 3.0f, col.size.z) * 0.5f + ghost.transform.position;
        corners[1] = col.center + new Vector3(-col.size.x, -col.size.y + 3.0f, col.size.z) * 0.5f + ghost.transform.position;
        corners[2] = col.center + new Vector3(-col.size.x, -col.size.y + 3.0f, -col.size.z) * 0.5f + ghost.transform.position;
        corners[3] = col.center + new Vector3(col.size.x, -col.size.y + 3.0f, -col.size.z) * 0.5f + ghost.transform.position;

        //center of bottom box collider
        Vector3 center = new Vector3(col.center.x + ghost.transform.position.x, corners[0].y, col.center.z + ghost.transform.position.z);

        corners[4] = center + (corners[0] - corners[1]) * 0.5f;
        corners[5] = center + (corners[1] - corners[2]) * 0.5f;
        corners[6] = center + (corners[2] - corners[3]) * 0.5f;
        corners[7] = center + (corners[3] - corners[0]) * 0.5f;

        corners[8] = center + (center - corners[0]) * 0.5f;
        corners[9] = center + (center - corners[1]) * 0.5f;
        corners[10] = center + (center - corners[2]) * 0.5f;
        corners[11] = center + (center - corners[3]) * 0.5f;

        //jst 4 debugging
        Debug.DrawRay(corners[0], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[1], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[2], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[3], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[4], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[5], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[6], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[7], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[8], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[9], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[10], Vector3.down * 10.0f, Color.red);
        Debug.DrawRay(corners[11], Vector3.down * 10.0f, Color.red);

        Debug.DrawRay(center, Vector3.down * 10.0f, Color.green);

        RaycastHit[] cornerHits = new RaycastHit[4];
        RaycastHit centerHit;

        Physics.Raycast(center, Vector3.down, out centerHit, 30.0f, 1 << 8);

        for (int i = 0; i < 4; ++i)
        {
            if (Physics.Raycast(corners[i], Vector3.down, out cornerHits[i], 30.0f, 1 << 8))
            {
                if (Mathf.Abs(centerHit.point.y - cornerHits[i].point.y) > marginOfDifference)
                    return false;
            }
            else
                return false;
        }

        if (intersects != 0)
            return false;

        return true;
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
