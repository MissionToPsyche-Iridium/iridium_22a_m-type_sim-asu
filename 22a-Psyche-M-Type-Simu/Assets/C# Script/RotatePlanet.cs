using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public float rotationSpeed = 10.0f; //can adjust in the Inspector

    void Update()
    {
        // Rotate the planet around its y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
