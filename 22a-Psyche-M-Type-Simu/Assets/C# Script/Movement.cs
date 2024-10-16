using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float speed = 5;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float HoriMove = Input.GetAxisRaw("Horizontal") * speed;
        float VertMove = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector3(HoriMove, rb.velocity.y, VertMove);

        if(Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0)) 
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);

    }
}

