using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP : MonoBehaviour
{

    [SerializeField] Transform Player;
    [SerializeField] float MouseSpeed = 12;
    [SerializeField] float orbitalDampen = 10;

    Vector3 LocalRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.position;

        if (Input.GetMouseButton(1))
        { 
            LocalRotation.x += Input.GetAxis("Mouse X") * MouseSpeed;
            LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSpeed;

            LocalRotation.y = Mathf.Clamp(LocalRotation.y, -30f, 50f);

            Quaternion qt = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, qt, Time.deltaTime * orbitalDampen);
        }
        /*
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z + .5f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z - .5f);
        }
        */
    }
}
