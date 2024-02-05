using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runhunt.Utils
{
    public class GameObjectHelper
    {

        private static GameObjectHelper s_instance;

        public static GameObjectHelper GetInstance()
        {

            if (s_instance == null)
                s_instance = new GameObjectHelper();
            return s_instance;

        }

        public List<GameObject> GetGameObjectsByLayerIdAndObjectName(int layer, string name)
        {

            List<GameObject> gameObjects = new List<GameObject>();
            List<GameObject> allGO = FindAllGameObjectsInScene();

            foreach (GameObject gameObject in allGO)
            {
                if (gameObject.layer == layer && gameObject.name == name)
                {
                    gameObjects.Add(gameObject);
                }
            }

            return gameObjects.Count == 0 ? null : gameObjects;
        }

        public static List<GameObject> FindAllGameObjectsInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            // iterate root objects and do something
            for (int i = 0; i < rootObjects.Count; ++i)
            {
                GameObject gameObject = rootObjects[i];
                objectsInScene.Add(gameObject);
            }
            return objectsInScene;
        }

    }
}