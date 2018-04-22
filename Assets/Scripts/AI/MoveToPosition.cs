using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class MoveToPosition : MonoBehaviour
    {
        [SerializeField]
        private Movement m_Movement;

        private Vector2? m_Target;

        public void SetTarget(Vector2 position)
        {
            m_Target = position;
        }

        public void ClearTarget()
        {
            m_Target = null;
        }

        private void Update()
        {
            if (m_Target == null)
                return;

            TryMoveTowardsTarget();
        }

        private void TryMoveTowardsTarget()
        {
            Debug.Assert(m_Target != null, nameof(m_Target) + " != null");

            Vector2 targetPosition = m_Target.Value;
            Vector2 myPosition = transform.position;
            Vector2 toTarget = targetPosition - myPosition;

            m_Movement.AddAcceleration(toTarget);
        }
    }
}