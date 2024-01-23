using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectHelper
{

    private static GameObjectHelper s_instance;

    public  static GameObjectHelper GetInstance()
    {
        if (s_instance == null)
            s_instance = new GameObjectHelper();
        return s_instance;
    }

    public List<GameObject> GetGameObjectsByLayerIdAndObjectName(int layer, string name)
    {
        
        List<GameObject> gameObjects = new List<GameObject> ();
        List<GameObject> allGO = FindAllGameObjectsInScene();

        foreach (GameObject gameObject in allGO)
        {
            if(gameObject.layer == layer && gameObject.name == name)
            {
                gameObjects.Add(gameObject);
            }
        }

        return gameObjects.Count == 0 ? null: gameObjects;
    }

    public List<GameObject> FindAllGameObjectsInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }

}
