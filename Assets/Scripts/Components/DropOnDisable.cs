﻿using UnityEngine;

namespace MightyPirates
{
    public sealed class DropOnDisable : MonoBehaviour
    {
        [SerializeField]
        private float m_Probability = 0.5f;

        private void OnDisable()
        {
            if (Random.value > m_Probability)
                return;

            Weapon loot = LootTable.GetLoot();
            if (loot != null)
            {
                GameObject drop = ObjectPool.Get(LootTable.DropPrefab, transform.position, Quaternion.identity);
                drop.GetComponent<Pickup>().Value = loot;
            }
        }
    }
}