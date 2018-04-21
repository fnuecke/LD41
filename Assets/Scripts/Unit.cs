using UnityEngine;

namespace MightyPirates
{
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField]
        private float m_ScanRadius;

        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private Weapon m_Weapon;

        private bool m_IsEnemy;
        private float m_Radius;

        private Transform m_Target;
        private float m_TargetRadius;

        private readonly Collider2D[] m_ScanResults = new Collider2D[16];

        private void Awake()
        {
            m_IsEnemy = CompareTag("Enemy");
            m_Radius = GetRadius(transform);
        }

        private void Update()
        {
            if (m_ScanRadius > 0 && m_Target == null)
            {
                SetTarget(ScanForTarget());
            }

            if (m_Target == null)
            {
                return;
            }

            TryRotateTowardsTarget();
            TryMoveTowardsTarget();
            TryAttackTarget();
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
            if (m_Target != null)
            {
                m_TargetRadius = GetRadius(m_Target);
            }
        }

        private Transform ScanForTarget()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, m_ScanRadius, m_ScanResults, ScanLayerMask);
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

            return bestTarget.transform;
        }

        private void TryRotateTowardsTarget()
        {
            Vector2 targetPosition = m_Target.position;
            Vector2 myPosition = transform.position;
            Vector2 toTarget = targetPosition - myPosition;

            m_Movement.SetLookVector(toTarget);
        }

        private void TryMoveTowardsTarget()
        {
            Vector2 targetPosition = m_Target.position;
            Vector2 myPosition = transform.position;
            Vector2 toTarget = targetPosition - myPosition;

            float distance = toTarget.magnitude;
            float desiredDistance = m_Weapon.Range * 0.8f;

            Vector2 acceleration = toTarget * (desiredDistance / distance);

            m_Movement.SetAcceleration(acceleration);
        }

        private void TryAttackTarget()
        {
            float distance = Vector2.Distance(m_Target.position, m_Weapon.transform.position);
            if (distance - m_Radius - m_TargetRadius < m_Weapon.Range)
            {
                m_Weapon.TryShoot();
            }
        }

        private static float GetRadius(Transform thing)
        {
            CircleCollider2D circleCollider = thing.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
                return circleCollider.radius;
            return 1f;
        }

        private int ScanLayerMask => m_IsEnemy ? Layers.PlayerMask : Layers.EnemyMask;

        private bool IsValidTarget(Transform target)
        {
            return target.GetComponent<Health>() != null;
        }
    }
}