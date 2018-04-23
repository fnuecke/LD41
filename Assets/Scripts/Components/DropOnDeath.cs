using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(Health))]
    public sealed class DropOnDeath : MonoBehaviour
    {
        [SerializeField]
        private float m_Probability = 0.5f;

        private void OnEnable()
        {
            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.Died += HandleDied;
            }
        }

        private void OnDisable()
        {
            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.Died -= HandleDied;
            }
        }

        private void HandleDied(GameObject source)
        {
            if (Random.value > m_Probability)
                return;

            Pickupable loot = LootTable.GetLoot();
            if (loot == null)
                return;

            GameObject drop = ObjectPool.Get(LootTable.DropPrefab, transform.position, Quaternion.identity);
            if (drop == null)
                return;

            drop.GetComponent<Pickup>().Value = loot;
        }
    }
}