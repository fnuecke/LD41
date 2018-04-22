using UnityEngine;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    public sealed class Healthbar : MonoBehaviour
    {
        [SerializeField]
        private Health m_Health;

        [SerializeField]
        private Image m_BarImage;

        [SerializeField]
        private Vector2 m_Offset = new Vector2(0, -1.5f);

        private void Awake()
        {
            if (m_Health == null)
                m_Health = GetComponent<Health>();
        }

        private void Update()
        {
            m_BarImage.fillAmount = m_Health.CurrentHealth / (float) m_Health.MaxHealth;
            transform.rotation = Quaternion.identity;
            transform.position = m_Health.transform.position + (Vector3) m_Offset;
        }
    }
}