using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMineExplotion : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_explotionSystem;
    [SerializeField]
    protected bool m_canBeAffected;
    [SerializeField]
    protected ETeamSide m_teamSide = ETeamSide.Count;
    [SerializeField]
    protected List<ETeamSide> m_affectedSide = new List<ETeamSide>();
    [SerializeField]
    private float m_deleteTimer = 1.6f;

    private void OnTriggerEnter(Collider other)
    {
        var otherHitBox = other.GetComponent<NetworkMineExplotion>();
        if (otherHitBox == null)
        {
            return;
        }

        if (CanInteract(otherHitBox))
        {
            Debug.Log(gameObject.name + " got hit by: " + otherHitBox);
            m_explotionSystem.SetActive(true);
            StartCoroutine(DeleteMine());
        }
    }

    IEnumerator DeleteMine()
    {
        yield return new WaitForSeconds(m_deleteTimer);
        Debug.Log("mine destroy");
        Destroy(this.gameObject);
    }

    protected bool CanInteract(NetworkMineExplotion other)
    {
        return (m_canBeAffected &&
            m_affectedSide.Contains(other.m_teamSide));
    }
}

public enum ETeamSide
{
    FloorTriger,
    Trap,
    Count
}