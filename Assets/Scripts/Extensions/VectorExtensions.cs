using UnityEngine;

namespace MightyPirates
{
    public static class VectorExtensions
    {
        public static Vector2Int ToVector2Int(this Vector2 self)
        {
            return new Vector2Int(Mathf.FloorToInt(self.x), Mathf.FloorToInt(self.y));
        }

        public static Vector3Int ToVector3Int(this Vector3 self)
        {
            return new Vector3Int(Mathf.FloorToInt(self.x), Mathf.FloorToInt(self.y), Mathf.FloorToInt(self.z));
        }

        public static Vector3Int ToVector3Int(this Vector2Int self)
        {
            return new Vector3Int(self.x, self.y, 0);
        }

        public static Vector3Int ToVector3Int(this Vector2 self)
        {
            return new Vector3Int(Mathf.FloorToInt(self.x), Mathf.FloorToInt(self.y), 0);
        }

        public static Vector3 ToVector3(this Vector2Int self, float z = 0)
        {
            return new Vector3(self.x + 0.5f, self.y + 0.5f, z);
        }

        public static Vector2 ToVector2(this Vector2Int self)
        {
            return new Vector2(self.x + 0.5f, self.y + 0.5f);
        }

        public static Vector3 ToVector3(this Vector3Int self)
        {
            return new Vector3(self.x + 0.5f, self.y + 0.5f, self.z);
        }
    }
}