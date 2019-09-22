using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    [SerializeField]
    private float speed = 0.0f;

    [SerializeField]
    private float offset = 0.0f;

    [SerializeField]
    private bool enable = false;

    private Camera cam = null;
    private float width = 0.0f;
    private float height = 0.0f;

    public float yOffset = 10.0f;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
    }
	
    void MoveCam()
    {
        width = Screen.width;
        height = Screen.height;

        if (Input.mousePosition.x >= (width - offset))
        {
           transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
        }
        else if (Input.mousePosition.x <= offset)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.mousePosition.y >= (height - offset))
        {
            transform.position += new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
        } 
        else if (Input.mousePosition.y <= offset)
        {
            transform.position -= new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -29.0f, 30.0f),
                                         yOffset,
                                         Mathf.Clamp(transform.position.z, -49.0f, 35.0f));
    }

	// Update is called once per frame
	void Update ()
    {
        if (enable)
        {
            MoveCam();
        }
	}
}
