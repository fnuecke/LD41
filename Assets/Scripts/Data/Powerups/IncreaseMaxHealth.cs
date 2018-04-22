using UnityEngine;

namespace MightyPirates.Powerups
{
    [CreateAssetMenu]
    public sealed class IncreaseMaxHealth : Powerup
    {
        [SerializeField]
        private int m_Amount = 50;

        public override void Activate(Player player)
        {
            Health health = player.GetComponent<Health>();
            if (health == null)
                return;

            health.MaxHealth += m_Amount;
        }
    }
}