using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    public GameObject disclaimerPanel; // assign this in the Inspector

    void Start()
    {
        if (PlayerPrefs.GetInt("ShowDisclaimer", 0) == 1)
        {
            disclaimerPanel.SetActive(true);
            PlayerPrefs.SetInt("ShowDisclaimer", 0); // reset it
        }
    }
}
