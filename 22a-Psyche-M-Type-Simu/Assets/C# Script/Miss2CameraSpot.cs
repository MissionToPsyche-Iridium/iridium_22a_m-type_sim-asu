using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miss2CameraSpot : MonoBehaviour, Miss2IInteractable
{
    [SerializeField] private string IntPrompt;
    public string InteractionPrompt { get; }


    public bool Interact(Miss2Interactor rover)
    {
        Debug.Log(message: "Camera Spot Located");
        return true;
    }
}