using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(TargetTracker))]
    public sealed class RotateToTarget : TargetTrackingBehaviour
    {
        [SerializeField]
        private Movement m_Movement;

        private void Update()
        {
            if (m_TargetTracker.Target == null)
                return;

            TryRotateTowardsTarget();
        }

        private void TryRotateTowardsTarget()
        {
            m_Movement.AddLookAt(m_TargetTracker.Target.transform.position);
        }
    }
}