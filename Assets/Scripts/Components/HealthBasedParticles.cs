using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(Health))]
    public sealed class HealthBasedParticles : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_DamagePrefab;

        [SerializeField]
        private GameObject m_DeathPrefab;

        private Health m_Health;

        private void Awake()
        {
            if (m_Health == null)
            {
                m_Health = GetComponent<Health>();
            }
        }

        private void OnEnable()
        {
            m_Health.DamageTaken += HandleDamageTaken;
            m_Health.Died += HandleDied;
        }

        private void OnDisable()
        {
            m_Health.DamageTaken -= HandleDamageTaken;
            m_Health.Died -= HandleDied;
        }

        private void HandleDamageTaken(GameObject source, int amount)
        {
            ObjectPool.Get(m_DamagePrefab, transform.position, Quaternion.identity);
        }

        private void HandleDied(GameObject source)
        {
            ObjectPool.Get(m_DeathPrefab, transform.position, Quaternion.identity);
        }
    }
}