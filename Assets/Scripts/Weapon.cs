using UnityEngine;

namespace MightyPirates
{
    public sealed class Weapon : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private float m_Frequency = 0.1f;

        [SerializeField]
        private float m_Range;

        private float m_TimeLastAttacked;

        public float Range => m_Range;

        public void TryShoot()
        {
            if (Time.time - m_TimeLastAttacked < m_Frequency) return;

            m_TimeLastAttacked = Time.time;
            ObjectPool.Get(m_Prefab, transform.position, transform.rotation);
        }
    }
}