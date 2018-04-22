using UnityEngine;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    public sealed class MinionCount : MonoBehaviour
    {
        [SerializeField]
        private Text m_Text;

        private Spawner m_PlayerSpawner;

        private void Update()
        {
            if (m_PlayerSpawner == null)
            {
                Player player = FindObjectOfType<Player>();
                if (player == null)
                    return;

                m_PlayerSpawner = player.GetComponentInChildren<Spawner>();
            }

            m_Text.text = $"{m_PlayerSpawner.Alive}/{m_PlayerSpawner.MaxAlive}";
        }
    }
}