using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
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

        private PooledObjectReference m_Target;
        private float m_TargetRadius;

        private readonly Collider2D[] m_ScanResults = new Collider2D[16];

        private void Awake()
        {
            m_IsEnemy = ((1 << gameObject.layer) & Layers.EnemyMask) != 0;
            m_Radius = GetRadius(gameObject);
        }

        private void Update()
        {
            if (m_ScanRadius > 0 && m_Target.Value == null)
            {
                SetTarget(ScanForTarget());
            }

            if (m_Target.Value == null)
            {
                m_Movement.Acceleration = Vector2.zero;
                return;
            }

            TryRotateTowardsTarget();
            TryMoveTowardsTarget();
            TryAttackTarget();
        }

        public void SetTarget(GameObject target)
        {
            m_Target = new PooledObjectReference(target);
            if (m_Target.Value != null)
            {
                m_TargetRadius = GetRadius(m_Target.Value);
            }
        }

        private GameObject ScanForTarget()
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

            return bestTarget.gameObject;
        }

        private void TryRotateTowardsTarget()
        {
            m_Movement.LookAt = m_Target.Value.transform.position;
        }

        private void TryMoveTowardsTarget()
        {
            Vector2 targetPosition = m_Target.Value.transform.position;
            Vector2 myPosition = transform.position;
            Vector2 toTarget = targetPosition - myPosition;

            float distance = toTarget.magnitude;
            float desiredDistance = m_Weapon.Range * 0.8f;

            Vector2 acceleration = toTarget * (desiredDistance / distance);

            m_Movement.Acceleration = acceleration;
        }

        private void TryAttackTarget()
        {
            float distance = Vector2.Distance(m_Target.Value.transform.position, m_Weapon.transform.position);
            if (distance - m_Radius - m_TargetRadius < m_Weapon.Range)
            {
                m_Weapon.TryShoot();
            }
        }

        private static float GetRadius(GameObject thing)
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