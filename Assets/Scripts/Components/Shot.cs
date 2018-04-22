using UnityEngine;

namespace MightyPirates
{
    public sealed class Shot : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_SpriteRenderer;

        [SerializeField]
        private Color m_PlayerColor = new Color(0.4f, 0.8f, 1f);

        [SerializeField]
        private Color m_EnemyColor = new Color(1f, 0.3f, 0.3f);

        public void Initialize(bool isEnemy)
        {
            if (m_SpriteRenderer != null)
            {
                m_SpriteRenderer.sortingLayerID = isEnemy ? SortingLayers.EnemyShots : SortingLayers.PlayerShots;
                m_SpriteRenderer.color = isEnemy ? m_EnemyColor : m_PlayerColor;
            }
            gameObject.SetLayerRecursive(isEnemy ? Layers.EnemyShots : Layers.PlayerShots);
        }
    }
}