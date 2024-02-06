using UnityEngine;

namespace Mirror
{
    public class CharacterData : MonoBehaviour
    {
        // A reference data script for most things character and customisation related.

        public static CharacterData characterDataSingleton { get; private set; }

        //public GameObject[] m_selectionPosePrefabs;
        public GameObject[] m_playablePrefabs;
        public string[] m_characterTitles;
        //public int[] characterHealths;
        //public float[] characterSpeeds;
        //public int[] characterAttack;
        //public string[] characterAbilities;

        public void Awake()
        {
            characterDataSingleton = this;
        }

    }

}