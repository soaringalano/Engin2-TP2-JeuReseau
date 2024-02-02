using UnityEngine;
using UnityEngine.UI;

public class RunnerUImanager : MonoBehaviour
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
    }

}
