using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Miss2IInteractable
{
    // Start is called before the first frame update
    public string InteractionPrompt { get; }

    public bool Interact(Miss2Interactor interactor);
}
