using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class DefendWhenAttacked : TargetTrackingBehaviour, ISpawnListener
    {
        private Health m_Health;
        private Guard m_Guard;
        private PooledObjectReference m_Spawner;

        protected override void Awake()
        {
            base.Awake();

            if (m_Health == null)
                m_Health = GetComponent<Health>();
            if (m_Guard == null)
                m_Guard = GetComponent<Guard>();
        }

        private void OnEnable()
        {
            if (m_Health != null)
            {
                m_Health.DamageTaken += HandleDamageTaken;
            }
            if (m_Guard != null)
            {
                GameObject guardTarget = m_Guard.Target;
                if (guardTarget != null)
                {
                    Health guardHealth = guardTarget.GetComponent<Health>();
                    if (guardHealth != null)
                    {
                        guardHealth.DamageTaken += HandleDamageTaken;
                    }
                }
            }
            if (m_Spawner.Value != null)
            {
                Health spawnerHealth = m_Spawner.Value.GetComponent<Health>();
                if (spawnerHealth != null)
                {
                    spawnerHealth.DamageTaken += HandleDamageTaken;
                }
            }
        }

        private void OnDisable()
        {
            if (m_Health != null)
            {
                m_Health.DamageTaken -= HandleDamageTaken;
            }
            if (m_Guard != null)
            {
                GameObject guardTarget = m_Guard.Target;
                if (guardTarget != null)
                {
                    Health guardHealth = guardTarget.GetComponent<Health>();
                    if (guardHealth != null)
                    {
                        guardHealth.DamageTaken -= HandleDamageTaken;
                    }
                }
            }
            if (m_Spawner.Value != null)
            {
                Health spawnerHealth = m_Spawner.Value.GetComponent<Health>();
                if (spawnerHealth != null)
                {
                    spawnerHealth.DamageTaken -= HandleDamageTaken;
                }
            }
        }

        public void HandleSpawned(GameObject spawner)
        {
            m_Spawner = new PooledObjectReference(spawner);
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (m_Spawner.Value != null)
            {
                Health spawnerHealth = m_Spawner.Value.GetComponent<Health>();
                if (spawnerHealth != null)
                {
                    spawnerHealth.DamageTaken += HandleDamageTaken;
                }
            }
        }

        private void HandleDamageTaken(GameObject source, int amount)
        {
            if (m_TargetTracker.Target != null)
            {
                return;
            }

            m_TargetTracker.Target = source;
        }
    }
}