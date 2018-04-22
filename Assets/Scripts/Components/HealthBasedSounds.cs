using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class HealthBasedSounds : MonoBehaviour
    {
        [SerializeField]
        private Sounds.SoundType m_DamageSound = Sounds.SoundType.TakeDamage;

        [SerializeField]
        private Sounds.SoundType m_DeathSound = Sounds.SoundType.SmallDeath;

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

        private void HandleDamageTaken(GameObject obj)
        {
            Sounds.Play(m_DamageSound, 0.8f);
        }

        private void HandleDied(GameObject obj)
        {
            Sounds.Play(m_DeathSound);
        }
    }
}