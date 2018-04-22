using UnityEngine;

namespace MightyPirates
{
    public sealed class Pickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon m_Value;

        public Weapon Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
    }
}