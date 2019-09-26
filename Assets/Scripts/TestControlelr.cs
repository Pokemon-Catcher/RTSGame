using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControlelr : MonoBehaviour
{
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime*speed, 0, Input.GetAxis("Vertical") *Time.deltaTime * speed);
    }
}
