using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:Assets/Scripts/Hunter/HunterMineExplosion.cs
public class HunterMineExplosion : NetworkBehaviour
{
    public static event Action<HunterMineExplosion> OnExplosionEvent;

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
        var otherHitBox = other.GetComponent<HunterMineExplosion>();
        if (otherHitBox == null)
=======
namespace Runhunt.Hunter
{
    public class HunterMineExplotion : NetworkBehaviour
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
>>>>>>> main:Assets/Scripts/Hunter/HunterMineExplotion.cs
        {
            var otherHitBox = other.GetComponent<HunterMineExplotion>();
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
<<<<<<< HEAD:Assets/Scripts/Hunter/HunterMineExplosion.cs
            if (OnExplosionEvent != null)
            {
                OnExplosionEvent(this);
            }
            Debug.Log(gameObject.name + " got hit by: " + otherHitBox);
            m_explotionSystem.SetActive(true);        
            StartCoroutine(DeleteMine());
=======
            yield return new WaitForSeconds(m_deleteTimer);
            Debug.Log("mine destroy");
            Destroy(this.gameObject);
        }

        protected bool CanInteract(HunterMineExplotion other)
        {
            return (m_canBeAffected &&
                m_affectedSide.Contains(other.m_teamSide));
>>>>>>> main:Assets/Scripts/Hunter/HunterMineExplotion.cs
        }
    }

    public enum ETeamSide
    {
        FloorTriger,
        Trap,
        Count
    }
<<<<<<< HEAD:Assets/Scripts/Hunter/HunterMineExplosion.cs

    protected bool CanInteract(HunterMineExplosion other)
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
=======
>>>>>>> main:Assets/Scripts/Hunter/HunterMineExplotion.cs
}