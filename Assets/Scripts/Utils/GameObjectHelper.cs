using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectHelper : MonoBehaviour
{

    public static GameObjectHelper s_instance;

    void Awake()
    {

        if (s_instance != null)
            Destroy(this);
        s_instance = this;

    }

    public List<GameObject> GetGameObjectsByLayerIdAndObjectName(int layer, string name)
    {

        List<GameObject> gameObjects = new List<GameObject> ();

        foreach (GameObject gameObject in gameObjects)
        {
            if(gameObject.layer == layer && gameObject.name == name)
            {
                gameObjects.Add(gameObject);
            }
        }

        return gameObjects.Count == 0 ? null: gameObjects;
    }

}
