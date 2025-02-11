using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCameraDetection : MonoBehaviour
{
    // Start is called before the first frame update
    Camera camera;
    MeshRenderer renderer;
    Plane[] CameraFrustum;
    Collider collider;

    void Start()
    {
        camera = Camera.main;
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        var bounds = collider.bounds;
        CameraFrustum = GeometryUtility.CalculateFrustumPlanes(camera);
        if (GeometryUtility.TestPlanesAABB(CameraFrustum, bounds))
        {
            renderer.sharedMaterial.color = Color.green;
        }
        else
        {
            renderer.sharedMaterial.color = Color.red;
        }
    }
}