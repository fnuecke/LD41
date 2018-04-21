using UnityEngine;

namespace MightyPirates
{
    public sealed class ForwardMover : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed = 1;

        private void Update()
        {
            transform.position += transform.up * m_Speed;
        }
    }
}