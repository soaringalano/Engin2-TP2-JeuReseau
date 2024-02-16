using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [field: SerializeField] public Scene SceneToLoad { get; set; }
    private GameObject panel;

    private void Start()
    {
        panel = GetComponentInChildren<RectTransform>().gameObject;
    }


    public void Play()
   {
        panel.SetActive(false);
        if (SceneToLoad == null) Debug.LogError("SceneToLoad is null");
        Debug.Log("SceneToLoad: " + SceneToLoad.ToString());
        LoadNextSceneByBuildIndex();
   }

    private void LoadNextSceneByBuildIndex()
    {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneBuildIndex);
        }
        else
        {
            Debug.Log("No next scene in build settings");
        }
    }

    //public void Options()
    // {
    //     Debug.Log("Options Button clicked");
    // }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

}
