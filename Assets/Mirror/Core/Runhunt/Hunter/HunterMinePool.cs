using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mirror
{
    public class HunterMinePool : NetworkBehaviour
    {
        [SerializeField] private GameObject m_minePrefab;
        private static Pool<GameObject> m_pool;
        private int m_currentCount;
        private const int MAX_MINES = 1000;

        public void Start()
        {
            m_pool = new Pool<GameObject>(CreatNewMineInPool, MAX_MINES);
        }

        private GameObject CreatNewMineInPool()
        {
            GameObject next = Instantiate(m_minePrefab, transform);

            // Remove NetworkIdentity from the instantiated GameObject, not from the original prefab
            NetworkIdentity networkIdentity = next.GetComponent<NetworkIdentity>();
            if (networkIdentity != null)
            {
                DestroyImmediate(networkIdentity);
            }

            next.name = $"{m_minePrefab.name}_pooled_{m_currentCount}";
            next.SetActive(false);
            m_currentCount++;
            return next;
        }

        public void CleanUpPoolAndFolder(string folderPath)
        {
            m_currentCount = 0;

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }
            foreach (GameObject child in children)
            {
                DestroyImmediate(child);
            }

            string[] assetPaths = AssetDatabase.FindAssets("", new[] { folderPath });
            foreach (string assetPath in assetPaths)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(assetPath);
                AssetDatabase.DeleteAsset(fullPath);
            }
            AssetDatabase.Refresh();
        }

        private GameObject CreateNew()
        {
            GameObject next = Instantiate(m_minePrefab, transform);
            next.name = $"{m_minePrefab.name}_pooled_{m_currentCount}";

#if UNITY_EDITOR
            // Ensure the prefab is saved with a unique name to prevent overwriting
            string uniquePrefabPath = $"{RoomManager.singleton.minePoolFolderPath}/{next.name}.prefab";
            // Check if a prefab with this unique name already exists to avoid unnecessary asset creation
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(uniquePrefabPath);
            if (existingPrefab == null)
            {
                // Save the instantiated object as a new prefab asset if one does not already exist
                PrefabUtility.SaveAsPrefabAsset(next, uniquePrefabPath);
                AssetDatabase.Refresh();
            }
#endif

            // Remove NetworkIdentity from the instantiated GameObject, not from the original prefab
            NetworkIdentity networkIdentity = next.GetComponent<NetworkIdentity>();
            if (networkIdentity != null)
            {
                DestroyImmediate(networkIdentity);
            }

            next.SetActive(false);
            m_currentCount++;
            return next;
        }

        [CustomEditor(typeof(HunterMinePool))]
        public class HunterMinePoolEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                HunterMinePool script = (HunterMinePool)target;

                if (GUILayout.Button("Initialize Pool"))
                {
                    if (!Application.isPlaying)
                    {
                        script.CleanUpPoolAndFolder(RoomManager.singleton.minePoolFolderPath);

                        m_pool = new Pool<GameObject>(script.CreateNew, HunterMinePool.MAX_MINES);
                    }
                    else
                    {
                        Debug.LogError("Pool initialization is only allowed in edit mode.");
                    }
                }

                if (GUILayout.Button("Delete mines"))
                {
                    if (!Application.isPlaying)
                    {
                        script.DeleteMines();
                    }
                    else
                    {
                        Debug.LogError("Child deletion is only allowed in edit mode.");
                    }
                }
            }
        }

        public void DeleteMines()
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }
            foreach (GameObject child in children)
            {
                DestroyImmediate(child); // Use DestroyImmediate in the editor, replace with Destroy if needed at runtime
            }
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            Debug.LogError("HunterMinePool: Get() called!");
            GameObject next = m_pool.Get(); // Makes unity editor not responding
            if (next != null)
            {
                next.transform.position = position;
                next.transform.rotation = rotation;
                next.SetActive(true);
            }
            return next;
        }

        protected void Return(GameObject spawned)
        {
            spawned.SetActive(false);
            m_pool.Return(spawned);
        }
    }
}