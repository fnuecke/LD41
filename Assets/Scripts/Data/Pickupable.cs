using UnityEngine;

namespace MightyPirates
{
    public abstract class Pickupable : ScriptableObject
    {
        [SerializeField]
        private string m_Title;

        [SerializeField]
        private string m_Description;

        public string Title => m_Title;
        public string Description => m_Description;
    }
}