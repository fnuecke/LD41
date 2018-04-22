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

        public int CurrentHealth => m_CurrentHealth;
        public int MaxHealth => m_MaxHealth;

        public event Action<GameObject> DamageTaken;
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
                OnDamageTaken(source);
            }
        }

        private void OnDeath(GameObject source)
        {
            OnDied(source);
            this.FreeGameObject();
        }

        private void OnDamageTaken(GameObject source)
        {
            DamageTaken?.Invoke(source);
        }

        private void OnDied(GameObject source)
        {
            Died?.Invoke(source);
        }
    }
}