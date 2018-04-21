using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class Guard : MonoBehaviour, ISpawnListener
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
        private TargetType m_TargetType;

        private Transform m_Target;

        private void OnEnable()
        {
            switch (m_TargetType)
            {
                case TargetType.Player:
                    m_Target = GameObject.FindGameObjectWithTag("Player").transform;
                    break;
            }
        }

        public void HandleSpawned(Spawner spawner)
        {
            switch (m_TargetType)
            {
                case TargetType.Spawner:
                    m_Target = spawner.transform;
                    break;
            }
        }

        private void Update()
        {
            TryMoveTowardsTarget();
        }

        private void TryMoveTowardsTarget()
        {
            Vector2 playerPosition = m_Target.position;
            Vector2 myPosition = transform.position;
            Vector2 toPlayer = playerPosition - myPosition;

            float distance = toPlayer.magnitude;
            Vector2 acceleration = toPlayer.normalized * (distance - m_Distance);

            m_Movement.AddAcceleration(acceleration);
        }
    }
}