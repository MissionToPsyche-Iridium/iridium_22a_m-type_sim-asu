using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCameraDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera camera2;
    MeshRenderer rendererV;
    Plane[] CameraFrustum;
    Collider colliderV;

    void Start()
    {
        //camera = Camera.main;
        rendererV = GetComponent<MeshRenderer>();
        colliderV = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        var bounds = colliderV.bounds;
        CameraFrustum = GeometryUtility.CalculateFrustumPlanes(camera2);
        if (GeometryUtility.TestPlanesAABB(CameraFrustum, bounds))
        {
            rendererV.sharedMaterial.color = Color.green;
        }
        else
        {
            rendererV.sharedMaterial.color = Color.red;
        }
    }
}