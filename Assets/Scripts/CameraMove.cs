using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    [SerializeField]
    private float maxSpeed = 0.0f;

    private Vector3 speed = Vector3.zero;

    [SerializeField]
    private float acceleration = 0.0f;

    [SerializeField]
    private float friction = 0.0f;

    [SerializeField]
    private float offset = 0.0f;

    [SerializeField]
    private float rotationMaxSpeed = 1.0f;

    private Vector2 rotationSpeed = Vector2.zero;

    [SerializeField]
    private float rotationAcceleration = 1.0f;

    [SerializeField]
    private float rotationMaxAcceleration = 1.0f;

    [SerializeField]
    private float rotationFriction = 1.0f;

    [SerializeField]
    private float distance = 0f;

    [SerializeField]
    private float minDistance = 0f;

    [SerializeField]
    private float maxDistance = 0f;


    [SerializeField]
    private float distanceScrollMaxSpeed = 0f;

    [SerializeField]
    private float distanceScrollAcceleration = 0f;

    [SerializeField]
    private float distanceScrollFriction = 0f;

    [SerializeField]
    private float maxAngleX = 0f;

    [SerializeField]
    private float minAngleX = 0f;

    [SerializeField]
    private bool enable = false;


    private Camera cam = null;
    private float width = 0.0f;
    private float height = 0.0f;
    private Vector3 point = Vector3.zero;
    private Vector3 mousePreviousPosition= Vector3.zero;
    private Vector3 deltaMousePosition = Vector3.zero;
    private Vector3 camMoveDirection = Vector3.zero;
    private Ray ray;
    private float actualDistance = 0f;
    private float scroll = 0f;
        private float distanceScrollSpeed = 0f;
    public float yOffset = 0f;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        mousePreviousPosition = Input.mousePosition;
        ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if(distance==0) distance = Vector3.Distance(ray.GetPoint(transform.position.y * Mathf.Cos(Vector3.Angle(ray.direction, Vector3.down))),transform.position);
    }
	
    void MoveCam()
    {
        width = Screen.width;
        height = Screen.height;
        camMoveDirection = Vector3.zero;
        ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        point = ray.GetPoint(transform.position.y * Mathf.Cos(Vector3.Angle(ray.direction, Vector3.down)));

        if (Input.GetButton("Fire3"))
        {
            deltaMousePosition = Input.mousePosition - mousePreviousPosition;
            if (Mathf.Abs(rotationSpeed.x) >= rotationMaxSpeed)
                rotationSpeed.x = Mathf.Sign(rotationSpeed.x) * rotationMaxSpeed;
            rotationSpeed.x += Mathf.Sign(deltaMousePosition.x) * (Mathf.Min(Mathf.Abs(deltaMousePosition.x) * rotationAcceleration, rotationMaxAcceleration)) * Time.deltaTime;
            if (Mathf.Abs(rotationSpeed.y) >= rotationMaxSpeed)
                rotationSpeed.y = Mathf.Sign(rotationSpeed.y) * rotationMaxSpeed;
            rotationSpeed.y += Mathf.Sign(deltaMousePosition.y) * (Mathf.Min(Mathf.Abs(deltaMousePosition.y) * rotationAcceleration, rotationMaxAcceleration)) * Time.deltaTime;

            transform.RotateAround(point, Vector3.up, rotationSpeed.x);
            //transform.RotateAround(point, transform.TransformDirection(Vector3.right), rotationSpeed.y);
        }
        else {
            if (Input.mousePosition.x >= (width - offset))
            {
                speed += transform.TransformDirection(Vector3.right) * acceleration * Time.deltaTime;
            }
            else if (Input.mousePosition.x <= offset)
            {
                speed -= transform.TransformDirection(Vector3.right) * acceleration * Time.deltaTime;
            }
            if (Input.mousePosition.y >= (height - offset))
            {
                speed += new Vector3(transform.forward.x,0,transform.forward.z).normalized * acceleration * Time.deltaTime;
            } 
            else if (Input.mousePosition.y <= offset)
            {
                speed -= new Vector3(transform.forward.x, 0, transform.forward.z).normalized * acceleration * Time.deltaTime;
            }
            Vector3 changeSpeed = speed.normalized * friction * Time.deltaTime;
            if (changeSpeed.magnitude > speed.magnitude)
                speed = Vector3.zero;
            else
                speed -= changeSpeed;
            if (speed.magnitude >= maxSpeed)
                speed = speed.normalized * maxSpeed;
            transform.position += speed;
        }

        if (Mathf.Abs(rotationSpeed.x) < rotationFriction * Time.deltaTime)
            rotationSpeed.x = 0;
        else
            rotationSpeed.x -= Mathf.Sign(rotationSpeed.x) * rotationFriction * Time.deltaTime;


        if (Mathf.Abs(rotationSpeed.y) < rotationFriction * Time.deltaTime)
            rotationSpeed.y = 0;
        else
            rotationSpeed.y -= Mathf.Sign(rotationSpeed.y) * rotationFriction * Time.deltaTime;


        scroll = Input.GetAxis("Mouse ScrollWheel");

        distanceScrollSpeed += (scroll * distanceScrollAcceleration) * Time.deltaTime;
        if (Mathf.Abs(distanceScrollSpeed) < friction * Time.deltaTime)
            distanceScrollSpeed = 0;
        else
            distanceScrollSpeed -= Mathf.Sign(distanceScrollSpeed)*friction*Time.deltaTime;
        if (Mathf.Abs(distanceScrollSpeed) > distanceScrollMaxSpeed)
            distanceScrollSpeed = Mathf.Sign(distanceScrollSpeed)*distanceScrollMaxSpeed;
        distance -= distanceScrollSpeed;
        if (distance > maxDistance)
            distance = maxDistance;
        else if (distance < minDistance)
            distance = minDistance;

        actualDistance= Vector3.Distance(ray.GetPoint(transform.position.y  * Mathf.Cos(Vector3.Angle(ray.direction, Vector3.down))), transform.position);
        if (distance != actualDistance) 
            transform.position += transform.forward*(actualDistance-distance);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -29.0f + (transform.position.x - point.x), 30.0f + (transform.position.x - point.x)),
                                 Mathf.Clamp (transform.position.y+yOffset,0f,60f),
                                 Mathf.Clamp(transform.position.z, -49.0f + (transform.position.z - point.z), 35.0f + (transform.position.z - point.z)));
        mousePreviousPosition = Input.mousePosition;
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
