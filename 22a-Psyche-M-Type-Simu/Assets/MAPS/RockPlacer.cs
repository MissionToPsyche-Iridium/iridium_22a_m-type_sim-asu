using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class RockPlacer : EditorWindow
{
    public GameObject[] rockPrefabs;
    public GameObject mapObject;
    public int numberOfRocks = 50;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;

    [MenuItem("Tools/Rock Placer")]
    public static void ShowWindow()
    {
        GetWindow<RockPlacer>("Rock Placer");
    }

    void OnGUI()
    {
        GUILayout.Label("Rock Placement Settings", EditorStyles.boldLabel);

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty rockPrefabsProperty = serializedObject.FindProperty("rockPrefabs");
        EditorGUILayout.PropertyField(rockPrefabsProperty, true);
        serializedObject.ApplyModifiedProperties();

        mapObject = (GameObject)EditorGUILayout.ObjectField("Map Object", mapObject, typeof(GameObject), true);
        numberOfRocks = EditorGUILayout.IntField("Number of Rocks", numberOfRocks);
        minScale = EditorGUILayout.FloatField("Min Scale (meters)", minScale);
        maxScale = EditorGUILayout.FloatField("Max Scale (meters)", maxScale);

        if (GUILayout.Button("Place Rocks"))
        {
            PlaceRocksOnMap();
        }
    }

    void PlaceRocksOnMap()
    {
        if (mapObject == null || rockPrefabs == null || rockPrefabs.Length == 0)
        {
            Debug.LogError("Please assign rock prefabs and a map object.");
            return;
        }

        // Get map collider
        Collider mapCollider = mapObject.GetComponent<Collider>();
        if (mapCollider == null)
        {
            Debug.LogError("Map object does not have a collider. Please ensure it has one.");
            return;
        }

        Vector3 mapMinBounds = mapCollider.bounds.min;
        Vector3 mapMaxBounds = mapCollider.bounds.max;

        // Counter to name rocks consecutively
        int rockCounter = 1;

        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(mapMinBounds.x, mapMaxBounds.x),
                mapMaxBounds.y + 5f, // Start a bit above the map
                Random.Range(mapMinBounds.z, mapMaxBounds.z)
            );

            GameObject rockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
            GameObject rock = Instantiate(rockPrefab, randomPosition, Quaternion.identity);

            float randomScale = Random.Range(minScale, maxScale);
            rock.transform.localScale = Vector3.one * randomScale;

            // Raycast down to find the surface of the map
            RaycastHit hit;
            if (Physics.Raycast(randomPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                rock.transform.position = hit.point; // Place rock on the surface
            }
            else
            {
                Destroy(rock); // Remove rock if it cannot be placed on the surface
                Debug.LogWarning("Rock could not be placed on the surface.");
                continue;
            }

            // Rename rock and parent it to the map for organization
            rock.name = "Rock_" + rockCounter++;
            rock.transform.parent = mapObject.transform;
        }

        Debug.Log("Rocks placed successfully!");
    }
}
#endif