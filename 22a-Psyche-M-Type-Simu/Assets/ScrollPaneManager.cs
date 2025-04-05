using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollPaneManager : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public Transform contentPanel;
    
    public void AddTextItem(string message) {
        GameObject newBox = Instantiate(textBoxPrefab, contentPanel);
        TMP_Text textComponent = newBox.GetComponentInChildren<TMP_Text>();

        if (textComponent != null) {
            textComponent.text = message;
        }
    }
}
