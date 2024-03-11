using Mirror;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomManager roomManager = (RoomManager)target;

        SerializedProperty spawnPrefabsProperty = serializedObject.FindProperty("spawnPrefabs");
        EditorGUILayout.PropertyField(spawnPrefabsProperty, new GUIContent("Spawn Prefabs"), true);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Update Mines in Spawnable Prefabs"))
        {
            // Remove empty or missing prefabs from the list
            roomManager.spawnPrefabs.RemoveAll(item => item == null);
            // Remove all GameObjects that incorrectly have the HunterMineExplosion component
            roomManager.spawnPrefabs.RemoveAll(go => go != null && go.GetComponent<HunterMineExplosion>() != null);

            var guids = AssetDatabase.FindAssets("t:GameObject", new[] { roomManager.minePoolFolderPath });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab.GetComponent<HunterMineExplosion>() != null && !roomManager.spawnPrefabs.Contains(prefab))
                {
                    roomManager.spawnPrefabs.Add(prefab);
                    Debug.Log($"Added {prefab.name} to spawnPrefabs");
                }
            }

            EditorUtility.SetDirty(roomManager);
        }
    }
}
