using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MightyPirates
{
    [DefaultExecutionOrder((int) ExectionOrders.Minimap)]
    public sealed class Minimap : MonoBehaviour
    {
        public static void Add(MinimapIcon minimapIcon)
        {
            if (s_Instance != null)
                s_Instance.AddInternal(minimapIcon);
        }

        public static void Remove(MinimapIcon minimapIcon)
        {
            if (s_Instance != null)
                s_Instance.RemoveInternal(minimapIcon);
        }

        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private Color m_PlayerColor = Color.green, m_EnemyColor = Color.red;

        private Dictionary<MinimapIcon, GameObject> m_Icons = new Dictionary<MinimapIcon, GameObject>();

        private static Minimap s_Instance;

        private void OnEnable()
        {
            s_Instance = this;
            if (m_Icons == null)
                m_Icons = new Dictionary<MinimapIcon, GameObject>();
            foreach (GameObject icon in m_Icons.Values)
                icon.Free();
            m_Icons.Clear();
            foreach (MinimapIcon minimapIcon in FindObjectsOfType<MinimapIcon>())
                AddInternal(minimapIcon);
        }

        private void OnDisable()
        {
            s_Instance = null;
        }

        private void Update()
        {
            foreach (KeyValuePair<MinimapIcon, GameObject> entry in m_Icons)
                ((RectTransform) entry.Value.transform).anchoredPosition = WorldToMinimapPosition(entry.Key.transform.position);
        }

        private void AddInternal(MinimapIcon minimapIcon)
        {
            if (!isActiveAndEnabled)
                return;
            if (m_Icons == null)
                m_Icons = new Dictionary<MinimapIcon, GameObject>();
            if (m_Icons.ContainsKey(minimapIcon))
                return;
            GameObject icon = ObjectPool.Get(m_Prefab, WorldToMinimapPosition(minimapIcon.transform.position), Quaternion.identity, transform);
            Image image = icon.GetComponent<Image>();
            image.overrideSprite = minimapIcon.Sprite;
            image.color = Layers.IsEnemy(minimapIcon.gameObject.layer) ? m_EnemyColor : m_PlayerColor;
            m_Icons.Add(minimapIcon, icon);
        }

        private void RemoveInternal(MinimapIcon minimapIcon)
        {
            if (!isActiveAndEnabled)
                return;
            if (m_Icons == null)
                return;
            GameObject icon;
            if (!m_Icons.TryGetValue(minimapIcon, out icon))
                return;
            m_Icons.Remove(minimapIcon);
            icon.Free();
        }

        private Vector2 WorldToMinimapPosition(Vector3 worldPosition)
        {
            Tilemap tilemap = TileTerrain.Instance.Tilemap;
            Vector2Int gridPosition = worldPosition.ToVector2Int(tilemap.cellSize);
            Vector2 relativePosition = new Vector2(gridPosition.x / (float) tilemap.size.x, gridPosition.y / (float) tilemap.size.y);
            Vector2 rectSize = ((RectTransform) transform).rect.size;
            return new Vector2(relativePosition.x * rectSize.x, relativePosition.y * rectSize.y);
        }
    }
}