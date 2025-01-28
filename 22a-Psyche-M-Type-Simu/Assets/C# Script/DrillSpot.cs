using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private string IntPrompt;
    public string InteractionPrompt { get; }

    
    public bool Interact(InteractorRover rover)
    {
        Debug.Log(message: "Drilling");
        return true;
    }
}
