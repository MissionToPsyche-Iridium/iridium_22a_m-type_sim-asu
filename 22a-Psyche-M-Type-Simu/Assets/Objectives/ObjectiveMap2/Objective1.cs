using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objective1 : MonoBehaviour
{
    public GameObject ObjectiveTextObject;
    public TextMeshProUGUI ObjectiveText;
    [SerializeField] public string Description;
    [SerializeField] public bool Complete; // Completion status
    public GameObject CompletedText;

    void Start()
    {
        Complete = false;

        if (ObjectiveText != null)
        {
            ObjectiveText.text = Description.ToString();
        }

        if (ObjectiveTextObject != null)
        {
            ObjectiveTextObject.SetActive(true); 
        }

        if (CompletedText != null)
        {
            CompletedText.SetActive(false); 
        }
    }
}