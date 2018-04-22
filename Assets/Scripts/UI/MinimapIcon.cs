using UnityEngine;

namespace MightyPirates.UI
{
    public sealed class MinimapIcon : MonoBehaviour
    {
        [SerializeField]
        private Sprite m_Sprite;

        public Sprite Sprite => m_Sprite;

        private void OnEnable()
        {
            Minimap.Add(this);
        }

        private void OnDisable()
        {
            Minimap.Remove(this);
        }
    }
}