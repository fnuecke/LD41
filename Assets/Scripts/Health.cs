using System;
using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class Health : MonoBehaviour
    {
        [SerializeField]
        private int m_MaxHealth;

        private int m_CurrentHealth;

        public int CurrentHealth => m_CurrentHealth;

        public event Action DamageTaken;
        public event Action Died;

        private void Awake()
        {
            m_CurrentHealth = m_MaxHealth;
        }

        public void ApplyDamage(int amount)
        {
            m_CurrentHealth -= amount;
            if (m_CurrentHealth <= 0)
            {
                OnDeath();
            }
            else
            {
                OnDamageTaken();
            }
        }

        private void OnDeath()
        {
            OnDied();
            Destroy(gameObject);
        }

        private void OnDamageTaken()
        {
            DamageTaken?.Invoke();
        }

        private void OnDied()
        {
            Died?.Invoke();
        }
    }
}