using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(TargetTracker))]
    public sealed class AttackTarget : TargetTrackingBehaviour
    {
        [SerializeField]
        private Weapon m_Weapon;

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
            Vector2 toTarget = m_TargetTracker.Target.transform.position - m_Weapon.transform.position;
            float angleDelta = Vector2.Angle(toTarget, m_Weapon.transform.up);
            if (angleDelta > 30)
                return;

            float distance = Vector2.Distance(m_TargetTracker.Target.transform.position, m_Weapon.transform.position);
            if (distance - m_Radius - m_TargetTracker.TargetRadius > m_Weapon.Range)
                return;

            m_Weapon.TryShoot();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(m_Weapon.transform.position, m_Weapon.transform.up);
        }
    }
}