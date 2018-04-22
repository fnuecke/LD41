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
                Vector3 position;
                if (!FindLegalPosition(out position))
                    continue;
                m_Path.Add(position);
            }

            int keyCount = m_Path.Count;
            if (keyCount > 1)
            {
                Tilemap tilemap = TileTerrain.Instance.Tilemap;
                Vector3 cellSize = tilemap.cellSize;
                for (int i = keyCount - 1; i > 0; i--)
                {
                    foreach (Vector2Int step in Pathfinding.FindPath(tilemap, m_Path[i - 1].ToVector2Int(cellSize), m_Path[i].ToVector2Int(cellSize)))
                    {
                        m_Path.Add(step.ToVector2(cellSize));
                    }
                    m_Path.RemoveAt(m_Path.Count - 1);
                }
                foreach (Vector2Int step in Pathfinding.FindPath(tilemap, m_Path[keyCount - 1].ToVector2Int(cellSize), m_Path[0].ToVector2Int(cellSize)))
                {
                    m_Path.Add(step.ToVector2(cellSize));
                }
                m_Path.RemoveRange(0, keyCount);
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
            m_Movement.AddLookAt(target);

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

        private bool FindLegalPosition(out Vector3 position)
        {
            Tilemap tilemap = TileTerrain.Instance.Tilemap;
            for (int j = 0; j < 20; j++)
            {
                position = (Vector2) transform.position + Random.insideUnitCircle * m_Radius;
                if (TileTerrain.Instance.IsLegalPosition(position) && Pathfinding.FindPath(tilemap, transform.position.ToVector2Int(tilemap.cellSize), position.ToVector2Int(tilemap.cellSize)) != null)
                    return true;
            }

            position = Vector3.zero;
            return false;
        }
    }
}