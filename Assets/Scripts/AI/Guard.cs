using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class Guard : TargetTrackingBehaviour, ISpawnListener
    {
        public enum TargetType
        {
            Player,
            Spawner,
        }

        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private float m_Distance = 2;

        [SerializeField]
        private float m_MaxDistance = 12;

        [SerializeField]
        private TargetType m_TargetType;

        private PooledObjectReference m_Target;

        public GameObject Target => m_Target.Value;

        private void OnEnable()
        {
            switch (m_TargetType)
            {
                case TargetType.Player:
                    m_Target = new PooledObjectReference(GameObject.FindGameObjectWithTag("Player"));
                    break;
            }
        }

        public void HandleSpawned(GameObject spawner)
        {
            switch (m_TargetType)
            {
                case TargetType.Spawner:
                    m_Target = new PooledObjectReference(spawner);
                    break;
            }
        }

        private void Update()
        {
            TryMoveTowardsTarget();
        }

        private void TryMoveTowardsTarget()
        {
            if (m_Target.Value == null)
            {
                return;
            }

            Vector2 targetPosition = m_Target.Value.transform.position;
            Vector2 myPosition = transform.position;
            Vector2 toTarget = targetPosition - myPosition;

            float distance = toTarget.magnitude;
            if (m_TargetTracker.Target != null && distance + Vector2.Distance(m_TargetTracker.Target.transform.position, myPosition) > m_MaxDistance * 2)
                m_TargetTracker.Target = null;

            Vector2 acceleration = toTarget.normalized * (distance - m_Distance);

            m_Movement.AddAcceleration(acceleration);
        }
    }
}