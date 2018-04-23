using System;
using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public sealed class Health : MonoBehaviour
    {
        [SerializeField]
        private int m_MaxHealth;

        private int m_CurrentHealth;

        public int CurrentHealth
        {
            get { return m_CurrentHealth; }
            set { m_CurrentHealth = Mathf.Clamp(value, 0, MaxHealth); }
        }

        public int MaxHealth
        {
            get { return m_MaxHealth; }
            set
            {
                float percent = m_CurrentHealth / (float) m_MaxHealth;
                m_MaxHealth = Mathf.Max(1, value);
                m_CurrentHealth = Mathf.Clamp(Mathf.CeilToInt(m_MaxHealth * percent), 0, m_MaxHealth);
            }
        }

        public event Action<GameObject, int> DamageTaken;
        public event Action<GameObject> Died;

        private void OnEnable()
        {
            m_CurrentHealth = m_MaxHealth;
        }

        private void OnDisable()
        {
            DamageTaken = null;
            Died = null;
        }

        public void ApplyDamage(GameObject source, int amount)
        {
            m_CurrentHealth -= amount;
            if (m_CurrentHealth <= 0)
            {
                OnDeath(source);
            }
            else
            {
                OnDamageTaken(source, amount);
            }
        }

        private void OnDeath(GameObject source)
        {
            OnDied(source);
            this.FreeGameObject();
        }

        private void OnDamageTaken(GameObject source, int amount)
        {
            DamageTaken?.Invoke(source, amount);
        }

        private void OnDied(GameObject source)
        {
            Died?.Invoke(source);
        }
    }
}