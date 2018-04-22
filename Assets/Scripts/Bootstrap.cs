using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace MightyPirates
{
    [DefaultExecutionOrder((int) ExectionOrders.Bootstrap)]
    public sealed class Bootstrap : MonoBehaviour
    {
        private const int SpawnGridSize = 8;

        [SerializeField]
        private TileTerrain m_Terrain;

        [SerializeField]
        private RenderTexture m_Minimap;

        [SerializeField]
        private Color m_ColorOpen = Color.white, m_ColorRock = Color.black;

        [SerializeField]
        private GameObject m_LoadingScreen;

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

        private readonly List<PooledObjectReference> m_GeneratedObjects = new List<PooledObjectReference>();

        private void Start()
        {
            GoalManager.GoalsChanged += HandleGoalsChanged;

            Time.timeScale = 0;
            BuildLevel();
        }

        private void HandleGoalsChanged()
        {
            if (GoalManager.GetGoalNames().Count == 0)
            {
                StartCoroutine(FadeInLoadingScreen());
            }
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

            // Prepare a grid of candidate positions:
            // +-------+-------+-------+
            // |   x   |   x   |   x   |
            // +-------+-------+-------+
            // |   x   |   x   |   x   |
            // +-------+-------+-------+
            // |   x   |   x   |   x   |
            // +-------+-------+-------+
            // Pretend this is a 24x24 grid, spawn pos interval is 8, so the
            // xs should be the candidate positions.
            BoundsInt bounds = m_Terrain.SafeBounds;
            List<Vector2Int> candidates = new List<Vector2Int>();
            int cols = bounds.size.x / SpawnGridSize;
            int rows = bounds.size.y / SpawnGridSize;
            for (int x = 0; x <= cols; x++)
            {
                for (int y = 0; y <= rows; y++)
                {
                    Vector2Int candidate = new Vector2Int(bounds.xMin + SpawnGridSize / 2 + x * SpawnGridSize, bounds.yMin + SpawnGridSize / 2 + y * SpawnGridSize);
                    candidates.Add(candidate);
                }
            }

            int underflow = 1 + m_EnemyPatrolCount + m_EnemyBaseCount - candidates.Count;
            if (underflow > 0)
            {
                int basesRemoved = Mathf.Min(underflow, m_EnemyBaseCount);
                m_EnemyBaseCount -= basesRemoved;
                underflow -= basesRemoved;
            }
            if (underflow > 0)
            {
                int patrolsRemoved = Mathf.Min(underflow, m_EnemyPatrolCount);
                m_EnemyBaseCount -= patrolsRemoved;
                underflow -= patrolsRemoved;
            }
            if (underflow > 0)
            {
                throw new Exception("Map too small to spawn even the player... welp!");
            }

            List<Vector2Int> shuffledCandidates = new List<Vector2Int>();
            for (;;)
            {
                retry:
                shuffledCandidates.Clear();
                shuffledCandidates.AddRange(candidates);
                shuffledCandidates.Shuffle();

                m_Terrain.GenerateTerrain();

                if (!FindLegalPosition(shuffledCandidates, out playerPosition))
                    goto retry;

                patrolPositions.Clear();
                for (int i = 0; i < m_EnemyPatrolCount; i++)
                {
                    Vector2Int patrolPosition;
                    if (!FindReachablePosition(playerPosition, shuffledCandidates, out patrolPosition))
                        goto retry;
                    patrolPositions.Add(patrolPosition);
                }

                basePositions.Clear();
                for (int i = 0; i < m_EnemyBaseCount; i++)
                {
                    Vector2Int basePosition;
                    if (!FindReachablePosition(playerPosition, shuffledCandidates, out basePosition))
                        goto retry;
                    basePositions.Add(basePosition);
                }

                break;
            }

            Vector3Int tilemapSize = m_Terrain.Tilemap.size;
            Texture2D minimap = new Texture2D(tilemapSize.x, tilemapSize.y, TextureFormat.ARGB32, false, true);
            minimap.filterMode = FilterMode.Point;
            try
            {
                for (int x = 0; x < tilemapSize.x; x++)
                {
                    for (int y = 0; y < tilemapSize.y; y++)
                    {
                        Color color = m_Terrain.Tilemap.GetColliderType(new Vector3Int(x, y, 0)) == Tile.ColliderType.None ? m_ColorOpen : m_ColorRock;
                        minimap.SetPixel(x, y, color);
                    }
                }

                minimap.Apply();
                Graphics.Blit(minimap, m_Minimap);
            }
            finally
            {
                Destroy(minimap);
            }

            Vector3 cellSize = m_Terrain.Tilemap.cellSize;
            m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(m_PlayerPrefab, playerPosition.ToVector3(cellSize, m_PlayerPrefab.transform.position.z), Quaternion.identity)));
            m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(m_CameraPrefab, playerPosition.ToVector3(cellSize, m_CameraPrefab.transform.position.z), Quaternion.identity)));
            if (m_EnemyPatrolPerfabs != null && m_EnemyPatrolPerfabs.Length > 0)
            {
                foreach (Vector2Int patrolPosition in patrolPositions)
                {
                    GameObject enemyPatrolPerfab = m_EnemyPatrolPerfabs[Random.Range(0, m_EnemyPatrolPerfabs.Length)];
                    m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(enemyPatrolPerfab, patrolPosition.ToVector3(cellSize, enemyPatrolPerfab.transform.position.z), Quaternion.identity)));
                }
            }
            if (m_EnemyBasePerfabs != null && m_EnemyBasePerfabs.Length > 0)
            {
                foreach (Vector2Int basePosition in basePositions)
                {
                    GameObject enemyBasePerfab = m_EnemyBasePerfabs[Random.Range(0, m_EnemyBasePerfabs.Length)];
                    m_GeneratedObjects.Add(new PooledObjectReference(ObjectPool.Get(enemyBasePerfab, basePosition.ToVector3(cellSize, enemyBasePerfab.transform.position.z), Quaternion.identity)));
                }
            }

            StartCoroutine(FadeOutLoadingScreen());
        }

        private const int LoadingScreenFadeSteps = 50;

        private IEnumerator FadeInLoadingScreen()
        {
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(0.5f);

            m_LoadingScreen.SetActive(true);
            CanvasGroup canvasGroup = m_LoadingScreen.GetOrAddComponent<CanvasGroup>();
            for (int i = 0; i < LoadingScreenFadeSteps; i++)
            {
                float progress = (i + 1) / (float) LoadingScreenFadeSteps;
                canvasGroup.alpha = progress;
                yield return null;
            }

            GC.Collect();
            RebuildLevel();
        }

        private IEnumerator FadeOutLoadingScreen()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            CanvasGroup canvasGroup = m_LoadingScreen.GetOrAddComponent<CanvasGroup>();
            for (int i = 0; i < LoadingScreenFadeSteps; i++)
            {
                float progress = i / (float) LoadingScreenFadeSteps;
                canvasGroup.alpha = 1 - progress;
                yield return null;
            }

            m_LoadingScreen.SetActive(false);
            Time.timeScale = 1;
        }

        private bool FindLegalPosition(List<Vector2Int> shuffledCandidates, out Vector2Int result)
        {
            if (shuffledCandidates.Count == 0)
            {
                result = Vector2Int.zero;
                return false;
            }

            for (int i = 0; i < 20; i++)
            {
                int positionIndex = Random.Range(0, shuffledCandidates.Count);
                result = shuffledCandidates[positionIndex] + (Random.insideUnitCircle * SpawnGridSize / 2).ToVector2Int(Vector3.one);
                if (m_Terrain.Tilemap.GetColliderType(result.ToVector3Int()) == Tile.ColliderType.None)
                {
                    shuffledCandidates.RemoveAt(positionIndex);
                    return true;
                }
            }

            result = Vector2Int.zero;
            return false;
        }

        private bool FindReachablePosition(Vector2Int start, List<Vector2Int> shuffledCandidates, out Vector2Int result)
        {
            for (int i = 0; i < 20; i++)
            {
                // Yes we consume the position either way, but if we can't reach it it's likely we won't get any other
                // useful positions from it, so whatever.
                if (FindLegalPosition(shuffledCandidates, out result) && Pathfinding.FindPath(m_Terrain.Tilemap, start, result) != null)
                    return true;
            }

            result = Vector2Int.zero;
            return false;
        }
    }
}