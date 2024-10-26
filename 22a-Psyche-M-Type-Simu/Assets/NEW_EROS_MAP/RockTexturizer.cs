using UnityEditor;
using UnityEngine;

public class RockTexturizer : EditorWindow
{
    public Material sharedMaterial;
    public string objectTag = "YourTag";

    [MenuItem("Tools/Apply Texture to Objects")]
    public static void ShowWindow()
    {
        GetWindow<RockTexturizer>("Apply Texture");
    }

    void OnGUI()
    {
        GUILayout.Label("Apply Texture to Multiple Objects", EditorStyles.boldLabel);
        sharedMaterial = (Material)EditorGUILayout.ObjectField("Material", sharedMaterial, typeof(Material), false);
        objectTag = EditorGUILayout.TextField("Object Tag", objectTag);

        if (GUILayout.Button("Apply Texture"))
        {
            ApplyMaterialToObjects();
        }
    }

    void ApplyMaterialToObjects()
    {
        if (sharedMaterial == null)
        {
            Debug.LogError("No Material assigned!");
            return;
        }

        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectTag);
        foreach (GameObject obj in objects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = sharedMaterial;
                EditorUtility.SetDirty(obj); 
            }
        }

        Debug.Log("Material applied to all objects with tag: " + objectTag);
    }
}
