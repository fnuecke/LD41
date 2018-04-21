using System;
using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public sealed class Damage : MonoBehaviour
    {
        [SerializeField]
        private int m_Damage;

        [SerializeField]
        private bool m_Once = true;

        public event Action AppliedDamage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            ApplyDamage(other);

            if (m_Once)
            {
                this.FreeGameObject();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            ApplyDamage(other);
        }

        private void ApplyDamage(Collider2D other)
        {
            OnAppliedDamage();

            Health health = other.GetComponent<Health>();
            if (health != null) health.ApplyDamage(m_Damage);
        }

        private void OnAppliedDamage()
        {
            AppliedDamage?.Invoke();
        }
    }
}