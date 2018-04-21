using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(TargetTracker))]
    public sealed class MoveToTarget : TargetTrackingBehaviour
    {
        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private float m_TargetDistance = 10f;

        public float TargetDistance
        {
            get { return m_TargetDistance; }
            set { m_TargetDistance = value; }
        }

        private void Update()
        {
            if (m_TargetTracker.Target == null)
                return;

            TryMoveTowardsTarget();
        }

        private void TryMoveTowardsTarget()
        {
            Vector2 targetPosition = m_TargetTracker.Target.transform.position;
            Vector2 myPosition = transform.position;
            Vector2 toTarget = targetPosition - myPosition;
            Vector2 acceleration = toTarget.normalized * (toTarget.magnitude - TargetDistance);

            m_Movement.AddAcceleration(acceleration);
        }
    }
}