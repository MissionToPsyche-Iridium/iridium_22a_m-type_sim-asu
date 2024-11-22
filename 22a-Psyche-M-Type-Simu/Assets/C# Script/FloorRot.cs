using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRot : MonoBehaviour
{
    private ClickMovement refcm;
    public Transform pos1;
    public Transform pos2;

    private float percent = 0f;
    private float scalar = .5f;

    private Quaternion oldNormal;
    private Quaternion startingRot;

    void Start()
    {
        startingRot = transform.rotation;
        oldNormal = startingRot;
        refcm = GetComponent<ClickMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Quaternion normal = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (Quaternion.Angle(oldNormal, normal) > 0.0001f)
            {
                newStart(normal);
            }
            //Debug.Log(hit.collider.name);
            Quaternion temp = new Quaternion();
            temp = Quaternion.Slerp(startingRot, normal, percent);

            //transform.LookAt(refcm.targetPosition);
            /*
            Vector3 lookDirection = refcm.targetPosition - transform.position;
            lookDirection.y = 0;
            Quaternion lookAtYRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = lookAtYRotation * Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward)) * temp;
            */

            transform.rotation = new Quaternion(temp.x, transform.rotation.y, temp.z, temp.w);
            //transform.rotation = Quaternion.Euler(temp.eulerAngles.x, lookAtYRotation.eulerAngles.y, temp.eulerAngles.z);
            //Quaternion temp1 = Quaternion.Slerp(startingRot, , percent);
            //transform.rotation = Quaternion.Euler(temp.eulerAngles.x, transform.rotation.eulerAngles.y, temp.eulerAngles.z);
            percent += Time.deltaTime * scalar;
            percent = Mathf.Clamp01(percent);
            
            if(percent >=1f)
            {
                startingRot = normal;
            }

            
            
        }
    }
    void newStart(Quaternion newNormal)
    {
        startingRot = transform.rotation;
        percent = 0f;
        oldNormal = newNormal;
    }
}