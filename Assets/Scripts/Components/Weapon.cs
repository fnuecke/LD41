using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    [CreateAssetMenu]
    public sealed class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private float m_Frequency = 0.1f;

        [SerializeField]
        private float m_Range;

        private readonly List<ISpawnListener> m_SpawnListeners = new List<ISpawnListener>();

        public float Range => m_Range;

        public void TryShoot(WeaponSlot slot, ref float timeLastAttacked)
        {
            if (Time.time - timeLastAttacked < m_Frequency) return;

            timeLastAttacked = Time.time;
            GameObject attack = ObjectPool.Get(m_Prefab, slot.transform.position, slot.transform.rotation);
            attack.layer = Layers.IsEnemy(slot.gameObject.layer) ? Layers.EnemyShots : Layers.PlayerShots;
            attack.GetComponents(m_SpawnListeners);
            if (m_SpawnListeners.Count > 0)
            {
                GameObject spawner = slot.GetComponentInParent<Entity>().gameObject;
                foreach (ISpawnListener listener in m_SpawnListeners)
                {
                    listener.HandleSpawned(spawner);
                }
            }
            m_SpawnListeners.Clear();
        }
    }
}