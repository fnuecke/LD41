using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MightyPirates
{
    public static class Pathfinding
    {
        private static readonly Vector2Int[] NeighborOffsets = {Vector2Int.up, Vector2Int.left, Vector2Int.right, Vector2Int.down};
        private static readonly HashSet<Vector2Int> Closed = new HashSet<Vector2Int>();
        private static readonly SortedList<float, Vector2Int> Open = new SortedList<float, Vector2Int>(ScoreComparer.Default);
        private static readonly Dictionary<Vector2Int, Vector2Int> CameFrom = new Dictionary<Vector2Int, Vector2Int>();
        private static readonly Dictionary<Vector2Int, float> GScore = new Dictionary<Vector2Int, float>();
        private static int s_Version;

        public static IEnumerable<Vector2Int> FindPath(Tilemap tilemap, Vector2Int start, Vector2Int goal)
        {
            if (!IsTileValid(tilemap, start) || !IsTileValid(tilemap, goal))
                return null;

            try
            {
                CameFrom.Clear();
                s_Version++;

                float startHeuristic = ComputeHeuristic(start, goal);
                GScore.Add(start, 0f);
                Open.Add(startHeuristic, start);

                while (Open.Count > 0)
                {
                    Vector2Int current = Open.Values[Open.Count - 1];
                    if (current == goal)
                        return ConstructPath(current);

                    Open.RemoveAt(Open.Count - 1);
                    Closed.Add(current);

                    foreach (Vector2Int offset in NeighborOffsets)
                    {
                        Vector2Int neighbor = current + offset;
                        if (!IsTileValid(tilemap, neighbor))
                            continue;

                        if (Closed.Contains(neighbor))
                            continue;

                        float tScore = GScore[current] + 1;

                        if (!Open.ContainsValue(neighbor))
                            Open.Add(tScore + ComputeHeuristic(neighbor, goal), neighbor);

                        float oScore;
                        if (GScore.TryGetValue(neighbor, out oScore) && oScore < tScore) // eh?
                            continue;

                        CameFrom[neighbor] = current;
                        GScore[neighbor] = tScore;
                    }
                }

                return null;
            }
            finally
            {
                Closed.Clear();
                Open.Clear();
                GScore.Clear();
            }
        }

        private static bool IsTileValid(Tilemap tilemap, Vector2Int tile)
        {
            return tilemap.cellBounds.Contains(tile.ToVector3Int()) && tilemap.GetColliderType(tile.ToVector3Int()) == Tile.ColliderType.None;
        }

        private static float ComputeHeuristic(Vector2Int start, Vector2Int goal)
        {
            return Vector2Int.Distance(start, goal);
        }

        private static IEnumerable<Vector2Int> ConstructPath(Vector2Int current)
        {
            int version = s_Version;
            do
            {
                yield return current;
                if (version != s_Version) throw new InvalidOperationException("Trying to read path after new call to FindPath.");
            } while (CameFrom.TryGetValue(current, out current));
        }

        private sealed class ScoreComparer : IComparer<float>
        {
            public static readonly ScoreComparer Default = new ScoreComparer();

            public int Compare(float x, float y)
            {
                int result = y.CompareTo(x);
                return result == 0 ? 1 : result;
            }
        }
    }
}