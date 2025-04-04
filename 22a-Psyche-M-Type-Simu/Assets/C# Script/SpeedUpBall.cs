using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBall : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private ClickMovement movementScript;
    //when the player makes contact with the sphere
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //increases speed and disables ball
            movementScript = playerObject.GetComponent<ClickMovement>();
            movementScript.moveSpeed += 0.5f;
            gameObject.SetActive(false);
        }
        
    }
}
