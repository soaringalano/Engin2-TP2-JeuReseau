using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [SerializeField]
    private Image m_filler;
    [SerializeField]
    private Button m_button;
    [SerializeField]
    private float m_cooldown = 5.0f;
    [SerializeField]
    private bool m_isInCooldown = false;
    [SerializeField]
    private bool m_buttonclick = false;

    void Start()
    {
        m_filler.fillAmount = 0.0f;
    }

    private void Update()
    {
        AbilitiesClick();
    }

    public void OnUseButton()
    {
        if (m_isInCooldown == true)
        {
            return;
        }
        m_buttonclick = true;
        AbilitiesClick();
    }

    private void AbilitiesClick()
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