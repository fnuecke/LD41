using UnityEngine;

namespace MightyPirates
{
    public abstract class Pickupable : ScriptableObject
    {
        [SerializeField]
        private Sprite m_Sprite;

        [SerializeField]
        private string m_Title;

        [SerializeField]
        private string m_Description;

        public Sprite Sprite => m_Sprite;
        public string Title => m_Title;
        public string Description => m_Description;
    }
}