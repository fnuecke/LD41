using UnityEngine;
using UnityEngine.Tilemaps;

namespace MightyPirates
{
    [CreateAssetMenu]
    public sealed class TileWithVariants : TileBase
    {
        [SerializeField]
        private Tile.ColliderType m_ColliderType = Tile.ColliderType.Sprite;

        [SerializeField]
        private Sprite[] m_Sprites;

        [SerializeField]
        private float m_PerlinScale = 1f;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, 100000f) * m_Sprites.Length), 0, m_Sprites.Length - 1);

            tileData.sprite = m_Sprites[index];
            tileData.color = Color.white;
            tileData.transform = Matrix4x4.identity;
            tileData.gameObject = null;
            tileData.flags = TileFlags.LockColor;
            tileData.colliderType = m_ColliderType;
        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset)
        {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }
    }
}