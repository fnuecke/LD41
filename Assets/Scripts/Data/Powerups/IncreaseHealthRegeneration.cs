using UnityEngine;

namespace MightyPirates.Powerups
{
    [CreateAssetMenu]
    public sealed class IncreaseHealthRegeneration : Powerup
    {
        [SerializeField]
        private int m_Amount = 5;

        public override void Activate(Player player)
        {
            HealthRegeneration regeneration = player.GetComponent<HealthRegeneration>();
            if (regeneration == null)
                return;

            regeneration.RegenerationAmount += m_Amount;
        }
    }
}