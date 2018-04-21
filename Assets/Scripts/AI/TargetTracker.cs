using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class TargetTracker : MonoBehaviour
    {
        private PooledObjectReference m_Target;

        public GameObject Target
        {
            get { return m_Target.Value; }
            set
            {
                m_Target = new PooledObjectReference(value);
                if (m_Target.Value != null)
                {
                    TargetRadius = m_Target.Value.GetRadius();
                }
            }
        }

        public float TargetRadius { get; private set; }

        private void Update()
        {
            if (m_Target.Value == null)
            {
                m_Target = default(PooledObjectReference);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Target.Value == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_Target.Value.transform.position, TargetRadius);
            Gizmos.DrawLine(transform.position, m_Target.Value.transform.position);
        }
    }
}