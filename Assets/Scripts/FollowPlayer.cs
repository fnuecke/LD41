using UnityEngine;

namespace MightyPirates
{
    public sealed class FollowPlayer : MonoBehaviour
    {
        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private float m_Distance = 32;

        private Transform m_Player;

        private void OnEnable()
        {
            m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            TryMoveTowardsPlayer();
        }

        private void TryMoveTowardsPlayer()
        {
            Vector2 playerPosition = m_Player.position;
            Vector2 myPosition = transform.position;
            Vector2 toPlayer = playerPosition - myPosition;

            float distance = toPlayer.magnitude;
            Vector2 acceleration = toPlayer.normalized * (distance - m_Distance);

            m_Movement.SetAcceleration(acceleration);
        }
    }
}