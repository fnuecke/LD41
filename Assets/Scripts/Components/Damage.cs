using System;
using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public sealed class Damage : MonoBehaviour, ISpawnListener
    {
        [SerializeField]
        private int m_Damage;

        [SerializeField]
        private bool m_Once = true;

        private PooledObjectReference m_Spawner;

        public GameObject Source => m_Spawner.Value != null ? m_Spawner.Value : gameObject;

        public event Action<GameObject> AppliedDamage;

        private void OnDisable()
        {
            AppliedDamage = null;
        }

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
            Entity otherEntity = other.GetComponentInParent<Entity>();
            OnAppliedDamage(otherEntity != null ? otherEntity.gameObject : other.gameObject);

            Health health = other.GetComponent<Health>();
            if (health != null) health.ApplyDamage(Source, m_Damage);
        }

        private void OnAppliedDamage(GameObject target)
        {
            AppliedDamage?.Invoke(target);
        }

        public void HandleSpawned(GameObject spawner)
        {
            m_Spawner = new PooledObjectReference(spawner);
        }
    }
}