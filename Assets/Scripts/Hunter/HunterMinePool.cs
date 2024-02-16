using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMinePool : NetworkBehaviour
{
    [SerializeField]private GameObject m_minePrefab;
    Pool<GameObject> m_pool;
    private int m_currentCount;
    public const int MAX_MINES = 1000;
    HunterFSM hfsm;
    Transform m_terrainTransform;

    public void Start()
    {
        m_pool = new Pool<GameObject>(CreateNew, MAX_MINES);
        hfsm = GetComponent<HunterFSM>();
        m_terrainTransform = hfsm.TerrainPlane.transform;
    }

    private GameObject CreateNew()
    {
        GameObject next = Instantiate(m_minePrefab, m_terrainTransform);
        m_minePrefab.transform.SetParent(next.transform);
        next.name = $"{m_minePrefab.name}_pooled_{m_currentCount}";
        next.SetActive(false);
        m_currentCount++;
        return next;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject next = m_pool.Get();
        //set position and rotation
        return next;
    }

    protected void Return(GameObject spawned)
    {
        //reset any state on the object
        spawned.SetActive(false);
        m_pool.Return(spawned);
    }
}
