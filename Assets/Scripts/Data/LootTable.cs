using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MightyPirates
{
    public sealed class LootTable : ScriptableObject
    {
        [SerializeField]
        private GameObject m_DropPrefab;

        [SerializeField]
        private LootEntry[] m_Loot;

        private static LootTable s_Instance;

        private List<LootEntry> m_OrderedLoot = new List<LootEntry>();
        private int m_WeightSum;

        public static GameObject DropPrefab => s_Instance.m_DropPrefab;

        public static Pickupable GetLoot()
        {
            return s_Instance.GetLootInternal();
        }

        private void OnEnable()
        {
            s_Instance = this;

            InitializeLookup();
        }

        private void OnDisable()
        {
            s_Instance = null;
        }

        private void InitializeLookup()
        {
            if (m_OrderedLoot == null)
                m_OrderedLoot = new List<LootEntry>();
            m_OrderedLoot.Clear();
            if (m_Loot != null) m_OrderedLoot.AddRange(m_Loot);
            m_OrderedLoot.Sort((a, b) => b.weight.CompareTo(a.weight));
            m_WeightSum = 0;
            foreach (LootEntry entry in m_OrderedLoot)
            {
                m_WeightSum += entry.weight;
            }
        }

        private Pickupable GetLootInternal()
        {
            int roll = Random.Range(0, m_WeightSum + 1);
            foreach (LootEntry entry in m_OrderedLoot)
            {
                roll -= entry.weight;
                if (roll <= 0)
                {
                    return entry.loot;
                }
            }

            return null;
        }

        [Serializable]
        private struct LootEntry
        {
            public int weight;
            public Pickupable loot;
        }
    }
}