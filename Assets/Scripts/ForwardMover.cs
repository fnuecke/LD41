using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class ForwardMover : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed = 1;

        private Rigidbody2D m_Body;

        private void Awake()
        {
            if (m_Body == null)
                m_Body = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            m_Body.velocity = transform.up * m_Speed;
        }
    }
}