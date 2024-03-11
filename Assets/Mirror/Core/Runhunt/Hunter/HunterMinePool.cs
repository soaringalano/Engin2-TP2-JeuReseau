using UnityEngine;

namespace Mirror
{
    public class HunterMinePool : NetworkBehaviour
    {
        [SerializeField] private GameObject m_minePrefab;
        Pool<GameObject> m_pool;
        private int m_currentCount;
        public const int MAX_MINES = 1000;

        public void Start()
        {
            m_pool = new Pool<GameObject>(CreateNew, MAX_MINES);
        }

        private GameObject CreateNew()
        {
            GameObject next = Instantiate(m_minePrefab, transform);
            next.name = $"{m_minePrefab.name}_pooled_{m_currentCount}";
            next.SetActive(false);
            m_currentCount++;
            return next;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            Debug.LogError("HunterMinePool: Get() called!");
            GameObject next = m_pool.Get(); // Makes unity editor not responding
            if (next != null)
            {
                next.transform.position = position;
                next.transform.rotation = rotation;
                next.SetActive(true);
            }
            return next;
        }

        protected void Return(GameObject spawned)
        {
            spawned.SetActive(false);
            m_pool.Return(spawned);
        }
    }
}