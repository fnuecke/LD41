using MightyPirates.UI;
using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(Health))]
    public sealed class StatTracker : MonoBehaviour
    {
        private bool m_IsEnemy;

        private void OnEnable()
        {
            m_IsEnemy = Layers.IsEnemy(gameObject.layer);

            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.DamageTaken += HandleDamageTaken;
                if (m_IsEnemy)
                    health.Died += HandleDied;
            }
        }

        private void OnDisable()
        {
            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.DamageTaken -= HandleDamageTaken;
                health.Died -= HandleDied;
            }
        }

        private void HandleDamageTaken(GameObject source, int amount)
        {
            if (m_IsEnemy) GameOver.AddDamageDealt(amount);
            else GameOver.AddMinionDamageTaken(amount);
        }

        private void HandleDied(GameObject source)
        {
            GameOver.AddEnemiesKilled(1);
        }
    }
}