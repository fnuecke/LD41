using System.Collections;
using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(Health))]
    public sealed class HealthRegeneration : MonoBehaviour
    {
        [SerializeField]
        private float m_Interval = 1;

        [SerializeField]
        private int m_Amount = 5;

        private Health m_Health;
        private Coroutine m_Coroutine;

        public int RegenerationAmount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }

        private void Awake()
        {
            if (m_Health == null)
                m_Health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            StartCoroutine(RegenerateHealth());
        }

        private void OnDisable()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        private IEnumerator RegenerateHealth()
        {
            for (;;)
            {
                yield return new WaitForSeconds(m_Interval);
                m_Health.CurrentHealth += m_Amount;
            }
        }
    }
}