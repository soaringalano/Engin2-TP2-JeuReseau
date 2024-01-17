using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class E01playerUImanager : MonoBehaviour
{
    public Slider slider;

    private void Update()
    {
        // Increment the variable when the 'i' key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
           slider.value++;
      
        }

        // Decrement the variable when the 'o' key is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            slider.value--;
        }
        else
        {
            Debug.Log("here goes the code for when the Player dies");
        }
    }

}
