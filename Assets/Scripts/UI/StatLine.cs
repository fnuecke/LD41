using UnityEngine;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    public sealed class StatLine : MonoBehaviour
    {
        [SerializeField]
        private Text m_Title;

        [SerializeField]
        private Text m_Value;

        public void SetTitle(string value) => m_Title.text = value;
        public void SetValue(string value) => m_Value.text = value;
    }
}