using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //float speed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(.05f, 0, 0) * Time.deltaTime * speed);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(-.05f, 0, 0) * Time.deltaTime * speed);
        } */

        if (Input.GetKey(KeyCode.W))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(100, 0, 0) * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(-100, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -100) * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 100) * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        }
    }

}

