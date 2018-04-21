using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(Camera))]
    public sealed class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Player m_Player;

        private Camera m_Camera;
        private Movement m_Movement;

        private void Awake()
        {
            if (m_Camera == null)
                m_Camera = GetComponent<Camera>();
            if (m_Movement == null)
                m_Movement = m_Player.GetComponent<Movement>();
        }

        private void Update()
        {
            Vector2 lookDirection = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 playerPosition = m_Player.transform.position;
            Vector2 playerLookVector = Vector2.ClampMagnitude(Vector2.Scale(lookDirection, new Vector2(1f / Screen.width, 1f / Screen.height)), 0.5f) * 2;
            Vector3 cameraPosition = playerPosition + playerLookVector;
            cameraPosition.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.5f);
        }
    }
}