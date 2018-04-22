using UnityEngine;

namespace MightyPirates
{
    public sealed class WeaponSlot : MonoBehaviour
    {
        [SerializeField]
        private Weapon m_Weapon;

        [SerializeField]
        private SpriteRenderer m_Visualization;

        private float m_TimeLastAttacked;

        public bool HasWeapon => m_Weapon != null;

        public float AttackAngle => m_Weapon != null ? m_Weapon.AttackAngle : 0f;
        public float Range => m_Weapon != null ? m_Weapon.Range : 0f;

        public Weapon Weapon
        {
            get { return m_Weapon; }
            set
            {
                m_Weapon = value;
                if (m_Visualization != null)
                {
                    m_Visualization.sprite = m_Weapon != null ? m_Weapon.Sprite : null;
                }
            }
        }

        public void TryShoot()
        {
            if (m_Weapon != null)
            {
                if (m_Weapon.TryShoot(this, ref m_TimeLastAttacked))
                {
                    Sounds.Play(m_Weapon.SoundType, 0.5f);
                }
            }
        }

        private void OnEnable()
        {
            if (m_Visualization != null)
            {
                m_Visualization.sprite = m_Weapon != null ? m_Weapon.Sprite : null;
            }
        }

        private void OnDrawGizmos()
        {
            if (HasWeapon)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, transform.up * Range);

                Gizmos.color = Color.yellow * 0.3f;
                Gizmos.DrawWireSphere(transform.position, Range);
            }
        }
    }
}