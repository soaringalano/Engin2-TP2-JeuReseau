using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private GameObject panel;

    private void Start()
    {
        panel = GetComponentInChildren<RectTransform>().gameObject;
    }


    public void Play()
   {
        panel.SetActive(false);
        SceneManager.LoadScene("002_OnlineLevel", LoadSceneMode.Single);
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
