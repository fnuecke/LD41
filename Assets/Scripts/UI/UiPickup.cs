using UnityEngine;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    public sealed class UiPickup : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Panel;

        [SerializeField]
        private Image m_PickupImage;

        [SerializeField]
        private Text m_PickupTitle;

        [SerializeField]
        private Text m_PickupDescription;

        [SerializeField]
        private GameObject[] m_EquipmentPanel;

        [SerializeField]
        private Image[] m_EquipmentImage;

        [SerializeField]
        private Text[] m_EquipmentTitle;

        [SerializeField]
        private Text[] m_EquipmentDescription;

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
                m_PickupImage.overrideSprite = pickup.Sprite;
                m_PickupTitle.text = pickup.Title;
                m_PickupDescription.text = pickup.Description;

                for (int i = 0; i < m_Player.Weapons.Count; i++)
                {
                    Weapon weapon = m_Player.Weapons[i].Weapon;
                    if (weapon == null)
                    {
                        m_EquipmentPanel[i].SetActive(false);
                    }
                    else
                    {
                        m_EquipmentImage[i].overrideSprite = pickup.Sprite;
                        m_EquipmentTitle[i].text = weapon.Title;
                        m_EquipmentDescription[i].text = weapon.Description;

                        m_EquipmentPanel[i].SetActive(true);
                    }
                }

                m_Panel.SetActive(true);
            }
        }
    }
}