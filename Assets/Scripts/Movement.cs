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

        private Vector2 m_Acceleration;
        private Vector2 m_LookVector;

        public void SetAcceleration(Vector2 acceleration)
        {
            m_Acceleration = acceleration;
        }

        public void SetLookVector(Vector2 value)
        {
            m_LookVector = value;
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

        private void HandleMovement()
        {
            Vector2 acceleration = m_Acceleration;

            float accelerationMagnitude = acceleration.magnitude;
            if (Mathf.Approximately(accelerationMagnitude, 0f)) return;
            acceleration = acceleration.normalized * Mathf.Clamp01(accelerationMagnitude) * m_MoveSpeed;

            m_Body.AddForce(acceleration, ForceMode2D.Force);
        }

        private void HandleRotation()
        {
            float targetAngle = Mathf.Atan2(m_LookVector.y, m_LookVector.x) * Mathf.Rad2Deg;
            float currentAngle = m_Body.rotation;
            float rotation = Mathf.DeltaAngle(currentAngle, targetAngle);
            if (rotation < -m_TurnSpeed) rotation = -m_TurnSpeed;
            else if (rotation > m_TurnSpeed) rotation = m_TurnSpeed;

            m_Body.MoveRotation(m_Body.rotation + rotation);
        }
    }
}