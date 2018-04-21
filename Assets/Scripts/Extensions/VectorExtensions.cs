using UnityEngine;

namespace MightyPirates
{
    public static class VectorExtensions
    {
        public static Vector2Int ToVector2Int(this Vector2 self, Vector3 cellSize)
        {
            return new Vector2Int(Mathf.FloorToInt(self.x / cellSize.x), Mathf.FloorToInt(self.y / cellSize.y));
        }

        public static Vector2 ToVector2(this Vector2Int self, Vector3 cellSize)
        {
            return new Vector2((self.x + 0.5f) * cellSize.x, (self.y + 0.5f) * cellSize.y);
        }

        public static Vector2Int ToVector2Int(this Vector3 self, Vector3 cellSize)
        {
            return new Vector2Int(Mathf.FloorToInt(self.x / cellSize.x), Mathf.FloorToInt(self.y / cellSize.y));
        }

        public static Vector3 ToVector3(this Vector2Int self, Vector3 cellSize, float z = 0)
        {
            return new Vector3((self.x + 0.5f) * cellSize.x, (self.y + 0.5f) * cellSize.y, z);
        }

        public static Vector3Int ToVector3Int(this Vector3 self, Vector3 cellSize)
        {
            return new Vector3Int(Mathf.FloorToInt(self.x / cellSize.x), Mathf.FloorToInt(self.y / cellSize.y), 0);
        }

        public static Vector3 ToVector3(this Vector3Int self, Vector3 cellSize)
        {
            return new Vector3((self.x + 0.5f) * cellSize.x, (self.y + 0.5f) * cellSize.y, 0);
        }

        public static Vector3Int ToVector3Int(this Vector2Int self)
        {
            return new Vector3Int(self.x, self.y, 0);
        }
    }
}