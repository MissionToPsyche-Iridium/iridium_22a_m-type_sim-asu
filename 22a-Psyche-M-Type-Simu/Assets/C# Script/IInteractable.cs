using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // Start is called before the first frame update
    public string InteractionPrompt { get; }

    public bool Interact(InteractorRover interactor);
}
