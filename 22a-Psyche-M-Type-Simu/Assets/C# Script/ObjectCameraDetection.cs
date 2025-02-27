using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCameraDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera camera2;
    [SerializeField] private GameObject target1;
    [SerializeField] private GameObject target2;
    [SerializeField] private GameObject target3;
    MeshRenderer rendererV;
    private Plane[] cameraFrustum;
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
        /*
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
        */
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(camera2);

        // Check visibility for each target
        UpdateTargetColor(target1);
        UpdateTargetColor(target2);
        UpdateTargetColor(target3);
    }

    void UpdateTargetColor(GameObject target)
    {
        if (target == null) return; // Ensure target exists

        Collider targetCollider = target.GetComponent<Collider>();
        MeshRenderer targetRenderer = target.GetComponent<MeshRenderer>();

        if (targetCollider != null && targetRenderer != null)
        {
            var bounds = targetCollider.bounds;
            if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            {
                targetRenderer.material.color = Color.green; // Target is visible
            }
            else
            {
                targetRenderer.material.color = Color.red; // Target is not visible
            }
        }
    }
}