using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class RockTagger : EditorWindow
{
    public string tagToApply = "Rock";

    [MenuItem("Tools/Tag Nested Rocks")]
    public static void ShowWindow()
    {
        GetWindow<RockTagger>("Tag Nested Rocks");
    }

    void OnGUI()
    {
        GUILayout.Label("Tag Nested Rocks", EditorStyles.boldLabel);
        tagToApply = EditorGUILayout.TextField("Tag", tagToApply);

        if (GUILayout.Button("Tag All Rocks"))
        {
            TagAllRocks();
        }
    }

    void TagAllRocks()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Rock"))
            {
                TagObjectAndChildren(obj, tagToApply);
            }
        }

        Debug.Log("Tagging complete.");
    }

    void TagObjectAndChildren(GameObject obj, string tag)
    {
        // Apply tag to the object itself
        obj.tag = tag;
        EditorUtility.SetDirty(obj); // Mark as dirty to save the changes

        // Recursively apply tag to all children
        foreach (Transform child in obj.transform)
        {
            TagObjectAndChildren(child.gameObject, tag);
        }
    }
}
#endif