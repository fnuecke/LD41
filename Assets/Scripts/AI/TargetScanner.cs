using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TargetTracker))]
    public sealed class TargetScanner : MonoBehaviour
    {
        [SerializeField]
        private float m_ScanRadius = 16;

        [SerializeField]
        private float m_ScanInterval = 0.2f;

        private int m_ScanLayerMask;
        private float m_LastScanTime;
        private TargetTracker m_TargetTracker;

        private readonly Collider2D[] m_ScanResults = new Collider2D[16];

        private void Awake()
        {
            bool isEnemy = ((1 << gameObject.layer) & Layers.EnemyMask) != 0;
            m_ScanLayerMask = isEnemy ? Layers.PlayerMask : Layers.EnemyMask;

            if (m_TargetTracker == null)
                m_TargetTracker = GetComponent<TargetTracker>();
        }

        private void Update()
        {
            if (m_TargetTracker.Target == null && m_ScanRadius > 0 && Time.time - m_LastScanTime > m_ScanInterval)
            {
                m_LastScanTime = Time.time;
                m_TargetTracker.Target = ScanForTarget();
            }
        }

        private GameObject ScanForTarget()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, m_ScanRadius, m_ScanResults, m_ScanLayerMask);
            if (hitCount == 0) return null;

            Transform bestTarget = null;
            float bestDist = 0f;

            int i = 0;
            for (; i < hitCount; ++i)
            {
                Transform target = m_ScanResults[i].transform;
                if (!IsValidTarget(target)) continue;
                bestTarget = target;
                bestDist = Vector2.Distance(bestTarget.transform.position, transform.position);
                break;
            }

            if (i == hitCount) return null;
            Debug.Assert(bestTarget != null);

            for (; i < hitCount; i++)
            {
                Transform target = m_ScanResults[i].transform;
                if (!IsValidTarget(target)) continue;

                float distance = Vector2.Distance(target.transform.position, transform.position);
                if (distance < bestDist)
                {
                    bestTarget = target;
                    bestDist = distance;
                }
            }

            return bestTarget.gameObject;
        }

        private bool IsValidTarget(Transform target)
        {
            return target.GetComponent<Health>() != null;
        }
    }
}