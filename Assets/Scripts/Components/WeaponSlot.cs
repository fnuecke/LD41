using UnityEngine;

namespace MightyPirates
{
    public sealed class WeaponSlot : MonoBehaviour
    {
        [SerializeField]
        private Weapon m_Weapon;

        private float m_TimeLastAttacked;

        public bool HasWeapon => m_Weapon != null;

        public float Range => m_Weapon != null ? m_Weapon.Range : 0f;

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