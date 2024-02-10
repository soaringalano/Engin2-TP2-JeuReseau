using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runhunt.Hunter
{
    public class HunterPowerUpButton : MonoBehaviour
    {
        protected HunterFSM m_stateMachine { get; set; }
        [field: SerializeField] private Image m_filler { get; set; }
        [field: SerializeField] private float m_cooldown { get; set; } = 5.0f;
        [field: SerializeField] private bool m_isInCooldown { get; set; } = false;
        [field: SerializeField] private bool m_buttonclick { get; set; } = false;

        virtual public void Start()
        {
            m_stateMachine = GetComponentInParent<HunterFSM>();
            m_filler.fillAmount = 0.0f;
        }

        virtual public void Update()
        {
            AbilitiesCooldown();
        }

        virtual public void OnUseButton()
        {
            Debug.Log("HunterPowerUpButton: OnUseButton.");
            if (m_isInCooldown == true)
            {
                return;
            }

            m_buttonclick = true;
            AbilitiesCooldown();
        }

        private void AbilitiesCooldown()
        {
            if (m_buttonclick == true)
            {
                m_buttonclick = false;
                m_isInCooldown = true;
                m_filler.fillAmount = 1.0f;
            }

            if (m_isInCooldown == true)
            {
                m_filler.fillAmount -= 1.0f / m_cooldown * Time.deltaTime;

                if (m_filler.fillAmount <= 0.0f)
                {
                    m_filler.fillAmount = 0.0f;
                    m_isInCooldown = false;
                }
            }
        }
    }
}