using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class RockTexturizer : EditorWindow
{
    public Material sharedMaterial;
    public string objectTag = "YourTag";
    public Vector2 textureTiling = new Vector2(1, 1);  // New tiling field

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
        textureTiling = EditorGUILayout.Vector2Field("Texture Tiling", textureTiling);  // Add tiling control

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

        // Set the tiling on the shared material itself
        sharedMaterial.mainTextureScale = textureTiling;

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
#endif