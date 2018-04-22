using UnityEngine;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    public sealed class UiPickup : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Panel;

        [SerializeField]
        private Text m_Title;

        [SerializeField]
        private Text m_Description;

        private Player m_Player;

        private void Update()
        {
            if (m_Player == null)
                m_Player = FindObjectOfType<Player>();
            if (m_Player == null)
            {
                SetPickup(null);
                return;
            }

            SetPickup(m_Player.Pickupable);
        }

        private void SetPickup(Pickupable pickup)
        {
            if (pickup == null)
            {
                m_Panel.SetActive(false);
            }
            else
            {
                m_Title.text = pickup.Title;
                m_Description.text = pickup.Description;
                m_Panel.SetActive(true);
            }
        }
    }
}