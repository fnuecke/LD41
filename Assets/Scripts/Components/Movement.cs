using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Movement : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_Body;

        [SerializeField]
        private float m_MoveSpeed = 10f;

        [SerializeField]
        private float m_TurnSpeed = 5f;

        private Vector2 m_AccelerationAcc;
        private int m_AccelerationCount;
        private Vector2 m_LookAtAcc;
        private int m_LookAtCount;

#if DEBUG
        private Vector2 m_LastAcceleration;
#endif

        public void AddAcceleration(Vector2 value)
        {
            m_AccelerationAcc += value;
            m_AccelerationCount++;
        }

        public void AddLookAt(Vector2 value)
        {
            m_LookAtAcc += value;
            m_LookAtCount++;
        }

        private void Awake()
        {
            if (m_Body == null)
                m_Body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

#if DEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, m_LastAcceleration);
        }
#endif

        private void HandleMovement()
        {
            if (m_AccelerationCount == 0) return;
            Vector2 acceleration = m_AccelerationAcc / m_AccelerationCount;
#if DEBUG
            m_LastAcceleration = acceleration;
#endif
            m_AccelerationAcc = Vector2.zero;
            m_AccelerationCount = 0;

            float accelerationMagnitude = acceleration.magnitude;
            if (Mathf.Approximately(accelerationMagnitude, 0f)) return;
            acceleration = acceleration.normalized * Mathf.Clamp01(accelerationMagnitude) * m_MoveSpeed;

            m_Body.AddForce(acceleration, ForceMode2D.Force);
        }

        private void HandleRotation()
        {
            if (m_LookAtCount == 0) return;
            Vector3 lookAt = m_LookAtAcc / m_LookAtCount;
            m_LookAtAcc = Vector2.zero;
            m_LookAtCount = 0;

            Vector2 toTarget = lookAt - transform.position;
            float targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            float currentAngle = m_Body.rotation;
            float rotation = Mathf.DeltaAngle(currentAngle, targetAngle);
            if (rotation < -m_TurnSpeed) rotation = -m_TurnSpeed;
            else if (rotation > m_TurnSpeed) rotation = m_TurnSpeed;

            m_Body.MoveRotation(m_Body.rotation + rotation);
        }
    }
}