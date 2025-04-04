using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BlueArrowFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        //moves with teh layer
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        //rotates with the player
        Quaternion lookRotation = Quaternion.LookRotation(player.forward, Vector3.up);
        Vector3 euler = lookRotation.eulerAngles;
        //rotates only the y cooridate to keep the arrow facing the +z and x rotated 90 degrees 
        transform.rotation = Quaternion.Euler(90f, euler.y, 0f); 
    }
}
