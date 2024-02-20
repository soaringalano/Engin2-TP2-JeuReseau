using System;
using System.Collections;
using UnityEngine;

namespace Mirror
{
    public class HunterMineExplosion : HunterMinePool
    {
        public static event Action<HunterMineExplosion> OnExplosionEvent;

        [SerializeField]
        private GameObject m_explotionSystem;
        [SerializeField]
        private float m_deleteTimer = 1.6f;

        private void OnTriggerEnter()
        {
            if (OnExplosionEvent != null)
            {
                OnExplosionEvent(this);
            }
            m_explotionSystem.SetActive(true);
            StartCoroutine(DeleteMine());
        }

        IEnumerator DeleteMine()
        {
            yield return new WaitForSeconds(m_deleteTimer);
            Debug.Log("mine destroy");
            base.Return(gameObject);
        }
    }
}