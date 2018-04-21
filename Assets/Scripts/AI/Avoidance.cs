using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    public sealed class Avoidance : MonoBehaviour
    {
        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private float m_MinDistance = 1f;

        [SerializeField]
        private float m_ScanInterval = 0.5f;

        [SerializeField]
        private float m_ScanRadius = 2f;

        [SerializeField]
        private LayerMask m_ScanMask;

        private Coroutine m_Coroutine;
        private readonly Collider2D[] m_ScanResults = new Collider2D[8];
        private readonly List<PooledObjectReference> m_Nearby = new List<PooledObjectReference>();

        private void OnEnable()
        {
            m_Coroutine = StartCoroutine(ScanPeriodically());
        }

        private void OnDisable()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_ScanRadius);
        }

        private void Update()
        {
            Vector2 myPosition = transform.position;
            foreach (PooledObjectReference reference in m_Nearby)
            {
                if (reference.Value == null) continue;
                Vector2 otherPosition = reference.Value.transform.position;
                float distance = (myPosition - otherPosition).sqrMagnitude;
                if (distance < m_MinDistance * m_MinDistance)
                {
                    distance = Mathf.Sqrt(distance);
                    Vector2 acceleration = (myPosition - otherPosition).normalized * (m_MinDistance - distance);
                    m_Movement.AddAcceleration(acceleration);
                }
            }
        }

        private IEnumerator ScanPeriodically()
        {
            for (;;)
            {
                ScanNow();
                yield return new WaitForSeconds(m_ScanInterval);
            }
        }

        private void ScanNow()
        {
            m_Nearby.Clear();

            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, m_ScanRadius, m_ScanResults, m_ScanMask);
            if (hitCount == 0)
                return;

            for (int i = 0; i < hitCount; i++)
            {
                m_Nearby.Add(new PooledObjectReference(m_ScanResults[i].gameObject));
            }
        }
    }
}