using UnityEngine;


public class PlayerData : MonoBehaviour
{
    public static PlayerData characterDataSingleton { get; private set; }

    public GameObject[] characterPrefabs;
    public string[] characterTitles;

    public void Awake()
    {
        characterDataSingleton = this;
    }
}
