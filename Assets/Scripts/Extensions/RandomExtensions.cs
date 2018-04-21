using UnityEngine;

namespace MightyPirates
{
    public static class RandomExtensions
    {
        public static Vector2Int Vector2Int(BoundsInt bounds)
        {
            return new Vector2Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax));
        }
    }
}