using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MapFunciton : MonoBehaviour
{
    [SerializeField] protected GameObject mapUI;
    [SerializeField] bool isMapEnabled;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cameraObject;
    private ClickMovement CMScript;
    private CameraControls CCScript;
    private float lastButtonPress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        isMapEnabled = false;
        mapUI.SetActive(false);

        CMScript = playerObject.GetComponent<ClickMovement>();
        CCScript = cameraObject.GetComponent<CameraControls>();
    }

    // Update is called once per frame
    void Update()
    {
        //if m button is pressed and has been greater than 0.3 seconds
        if (Input.GetKey(KeyCode.M) && (Time.unscaledTime - lastButtonPress > 0.3f))
        {
            if (isMapEnabled)
            { //closes map and unfreezes the scene when m is pressed
                mapUI.SetActive(false);
                isMapEnabled = false;
                Time.timeScale = 1f;
                lastButtonPress = Time.unscaledTime;
                CMScript.enabled = true;
                CCScript.enabled = true;
            }
            else
            { //opens map and freezes scene whenm m is pressed
                mapUI.SetActive(true);
                isMapEnabled = true;
                Time.timeScale = 0f;
                lastButtonPress = Time.unscaledTime;
                CMScript.enabled = false;
                CCScript.enabled = false;
            }
        }
    }
}
