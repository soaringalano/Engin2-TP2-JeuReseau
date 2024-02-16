using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HunterMineFollowPivot : MonoBehaviour
{
    Scene scene;
    Transform sceneTransform;
    Transform environement;
    Transform runnerPlatform;
    Transform runnerFloorPlatform;
    void Start()
    {
        // Source : https://discussions.unity.com/t/find-gameobjects-in-specific-scene-only/163901
        scene = gameObject.scene;
        GameObject[] gameObjects = scene.GetRootGameObjects();
        sceneTransform = null;

        foreach (GameObject _gameObject in gameObjects)
        {
            if (_gameObject.name != "Scene") continue;

            sceneTransform = _gameObject.transform;
            break;
        }

        if (sceneTransform == null)
        {
            Debug.LogError("First scene child GameObject not found!");
            return;
        }

        environement = sceneTransform.GetChild(0);
        if (environement.name != "Environment")
        {
            Debug.LogError("Please place Environment GameObject as first child in the scene! First scene child GameObject name: " + sceneTransform.name);
            return;
        }

        runnerPlatform = environement.GetChild(0);
        if (runnerPlatform.name != "RunnerPlatform")
        {
            Debug.LogError("Please place RunnerPlatform GameObject as first child in Environment! Cureent GO is: " + runnerPlatform.name);
            return;
        }

        runnerFloorPlatform = runnerPlatform.GetChild(0);
        if (runnerFloorPlatform.name != "RunnerFloorPlatform")
        {
            Debug.LogError("Please place RunnerFloorPlatform GameObject as first child in RunnerPlatform! Cureent GO is: " + runnerFloorPlatform.name);
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
