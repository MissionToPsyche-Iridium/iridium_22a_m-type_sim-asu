using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMapObjective : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private GameObject greenCircle;

    // Update is called once per frame
    void Update()
    {
        if (interactableObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            greenCircle.SetActive(true);
        }
        else
        {
            greenCircle.SetActive(false);
        }
    }
}
