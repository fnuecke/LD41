using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TargetTracker))]
    public sealed class AttackTarget : TargetTrackingBehaviour
    {
        [SerializeField]
        private WeaponSlot[] m_WeaponSlots;

        private float m_Radius;

        protected override void Awake()
        {
            base.Awake();
            m_Radius = gameObject.GetRadius();
        }

        private void Update()
        {
            if (m_TargetTracker.Target == null)
                return;

            TryAttackTarget();
        }

        private void TryAttackTarget()
        {
            if (m_WeaponSlots == null || m_WeaponSlots.Length == 0)
                return;

            foreach (WeaponSlot weaponSlot in m_WeaponSlots)
            {
                if (!weaponSlot.HasWeapon)
                    continue;

                Vector2 toTarget = m_TargetTracker.Target.transform.position - weaponSlot.transform.position;
                float angleDelta = Vector2.Angle(toTarget, weaponSlot.transform.up);
                if (angleDelta > weaponSlot.AttackAngle)
                    continue;

                float distance = Vector2.Distance(m_TargetTracker.Target.transform.position, weaponSlot.transform.position);
                if (distance - m_Radius - m_TargetTracker.TargetRadius > weaponSlot.Range)
                    continue;

                weaponSlot.TryShoot();
            }
        }
    }
}