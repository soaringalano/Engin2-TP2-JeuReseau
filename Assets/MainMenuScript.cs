using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
   public void Play()
   {
        SceneManager.LoadScene("002_OnlineLevel", LoadSceneMode.Single);
   }

   public void Options()
    {
        Debug.Log("Options Button clicked");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

}
