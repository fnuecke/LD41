using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MightyPirates
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private TileTerrain m_Terrain;

        [SerializeField]
        private GameObject m_PlayerPrefab;

        [SerializeField]
        private GameObject m_CameraPrefab;

        [SerializeField]
        private GameObject[] m_EnemyPatrolPerfabs;

        [SerializeField]
        private GameObject[] m_EnemyBasePerfabs;

        [SerializeField]
        private int m_EnemyPatrolCount;

        [SerializeField]
        private int m_EnemyBaseCount;

        private List<PooledObjectReference> m_GeneratedObjects = new List<PooledObjectReference>();

        private void Start()
        {
            BuildLevel();
        }

        public void RebuildLevel()
        {
            ClearLevel();
            BuildLevel();
        }

        private void ClearLevel()
        {
            foreach (PooledObjectReference reference in m_GeneratedObjects)
                reference.Free();
            m_GeneratedObjects.Clear();
        }

        private void BuildLevel()
        {
            Vector2Int playerPosition;
            List<Vector2Int> patrolPositions = new List<Vector2Int>();
            List<Vector2Int> basePositions = new List<Vector2Int>();

            for (;;)
            {
                m_Terrain.GenerateTerrain();

                BoundsInt bounds = m_Terrain.SafeBounds;
                if (!FindLegalPosition(bounds, out playerPosition))
                    continue;

                patrolPositions.Clear();
                for (int i = 0; i < m_EnemyPatrolCount; i++)
                {
                    Vector2Int patrolPosition;
                    if (!FindReachablePosition(bounds, playerPosition, out patrolPosition))
                        continue;
                    patrolPositions.Add(patrolPosition);
                }

                basePositions.Clear();
                for (int i = 0; i < m_EnemyBaseCount; i++)
                {
                    Vector2Int basePosition;
                    if (!FindReachablePosition(bounds, playerPosition, out basePosition))
                        continue;
                    basePositions.Add(basePosition);
                }

                break;
            }

            m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(m_PlayerPrefab, playerPosition.ToVector3(m_PlayerPrefab.transform.position.z), Quaternion.identity)));
            m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(m_CameraPrefab, playerPosition.ToVector3(m_CameraPrefab.transform.position.z), Quaternion.identity)));
            if (m_EnemyPatrolPerfabs != null && m_EnemyPatrolPerfabs.Length > 0)
            {
                foreach (Vector2Int patrolPosition in patrolPositions)
                {
                    GameObject enemyPatrolPerfab = m_EnemyPatrolPerfabs[Random.Range(0, m_EnemyPatrolPerfabs.Length)];
                    m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(enemyPatrolPerfab, patrolPosition.ToVector3(enemyPatrolPerfab.transform.position.z), Quaternion.identity)));
                }
            }
            if (m_EnemyBasePerfabs != null && m_EnemyBasePerfabs.Length > 0)
            {
                foreach (Vector2Int basePosition in basePositions)
                {
                    GameObject enemyBasePerfab = m_EnemyBasePerfabs[Random.Range(0, m_EnemyBasePerfabs.Length)];
                    m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(enemyBasePerfab, basePosition.ToVector3(enemyBasePerfab.transform.position.z), Quaternion.identity)));
                }
            }
        }

        private bool FindLegalPosition(BoundsInt bounds, out Vector2Int result)
        {
            for (int i = 0; i < 20; i++)
            {
                result = RandomExtensions.Vector2Int(bounds);
                if (m_Terrain.Tilemap.GetColliderType(result.ToVector3Int()) == Tile.ColliderType.None)
                    return true;
            }

            result = Vector2Int.zero;
            return false;
        }

        private bool FindReachablePosition(BoundsInt bounds, Vector2Int start, out Vector2Int result)
        {
            for (int i = 0; i < 20; i++)
            {
                if (FindLegalPosition(bounds, out result) && Pathfinding.FindPath(m_Terrain.Tilemap, start, result) != null)
                    return true;
            }

            result = Vector2Int.zero;
            return false;
        }
    }
}