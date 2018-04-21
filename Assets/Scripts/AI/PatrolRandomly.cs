using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class PatrolRandomly : MonoBehaviour
    {
        [SerializeField]
        private Movement m_Movement;

        [SerializeField]
        private float m_Radius;

        private List<Vector2> m_Path = new List<Vector2>();
        private int m_PathIndex;

        private void Awake()
        {
            if (m_Movement == null)
                m_Movement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            if (m_Path == null)
                m_Path = new List<Vector2>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Vector2 position = (Vector2) transform.position + Random.insideUnitCircle * m_Radius;
                    Tilemap tilemap = TileTerrain.Instance.Tilemap;
                    if (tilemap.GetColliderType(position.ToVector3Int()) != Tile.ColliderType.None)
                        continue;
                    m_Path.Add(position);
                    break;
                }
            }
            m_PathIndex = 0;
        }

        private void OnDisable()
        {
            m_Path.Clear();
            m_PathIndex = 0;
        }

        private void Update()
        {
            Vector3 target = m_Path[m_PathIndex];
            Vector2 toTarget = target - transform.position;
            m_Movement.AddAcceleration(toTarget);

            if (toTarget.sqrMagnitude < 1)
            {
                m_PathIndex = (m_PathIndex + 1) % m_Path.Count;
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Path == null || m_Path.Count < 2)
                return;

            Gizmos.color = Color.cyan;
            for (int i = 1; i < m_Path.Count; i++)
                Gizmos.DrawLine(m_Path[i - 1], m_Path[i]);
            Gizmos.DrawLine(m_Path[m_Path.Count - 1], m_Path[0]);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
    }
}