using UnityEngine;

namespace MightyPirates
{
    public static class BoundsIntExtensions
    {
        public static BoundsInt Expand(this BoundsInt self, Vector3Int delta)
        {
            return new BoundsInt(self.position - new Vector3Int(delta.x, delta.y, delta.z), new Vector3Int(self.size.x + delta.x * 2, self.size.y + delta.y * 2, self.size.z + delta.z * 2));
        }
    }
}