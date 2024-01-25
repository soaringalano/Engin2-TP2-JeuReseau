using Mirror;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void OnTriggerEnter(Collider other)
    {
        var otherHitBox = other.GetComponent<NetworkMineExplotion>();
        if (otherHitBox == null )
        {
            return; 
        }

        if (CanInteract(otherHitBox))
        {
            Debug.Log(gameObject.name + " got hit by: " + otherHitBox);
           m_explotionSystem.SetActive(true);
        }       
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