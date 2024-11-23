using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Objective1Complete : MonoBehaviour
{
    public bool Complete;
    public string TextComplete = "Objective Complete!";
    public TextMeshProUGUI Text;

    void Start()
    {
        if (Text != null)
        {
            Text.text = "";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Complete)
        {
            Debug.Log("Player collided with the objective marker!");
            Complete = true; // Mark as complete
            Text.text = TextComplete; // Display completion message
            StartCoroutine(WaitForSec());
        }
    }

    public IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(2);
        if (Text != null)
        {
            Destroy(Text.gameObject); // Destroy the text UI object
        }
        Destroy(this.gameObject);
    }
}