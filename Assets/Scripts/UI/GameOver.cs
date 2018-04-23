using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    public sealed class GameOver : MonoBehaviour
    {
        public static void AddDamageDealt(int value)
        {
            s_Instance.m_DamageDealt += value;
        }

        public static void AddDamageTaken(int value)
        {
            s_Instance.m_DamageTaken += value;
        }

        public static void AddMinionDamageTaken(int value)
        {
            s_Instance.m_MinionDamageTaken += value;
        }

        public static void AddEnemiesKilled(int value)
        {
            s_Instance.m_EnemiesKilled += value;
        }

        public static void ShowStats()
        {
            s_Instance.ShowStatsInternal();
        }

        private static GameOver s_Instance;

        [SerializeField]
        private RectTransform m_Container;

        [SerializeField]
        private RectTransform m_StatContainer;

        [SerializeField]
        private GameObject m_StatLine;

        [SerializeField]
        private Image m_Curtain;

        private long m_DamageDealt;
        private long m_DamageTaken;
        private long m_MinionDamageTaken;
        private long m_EnemiesKilled;

        [UsedImplicitly]
        public void ReturnToMainMenu()
        {
            StartCoroutine(FadeToMainMenu());
        }

        private void OnEnable()
        {
            s_Instance = this;
        }

        private void OnDisable()
        {
            s_Instance = null;
        }

        private void ShowStatsInternal()
        {
            AddStatLine("Enemies killed", m_EnemiesKilled.ToString());
            AddStatLine("Damage dealt", m_DamageDealt.ToString());
            AddStatLine("Damage taken", m_DamageTaken.ToString());
            AddStatLine("Minion damage taken", m_MinionDamageTaken.ToString());

            StartCoroutine(FadeInStatScreen());
        }

        private void AddStatLine(string title, string value)
        {
            StatLine statLine = Instantiate(m_StatLine, m_StatContainer, false).GetComponent<StatLine>();
            statLine.SetTitle(title);
            statLine.SetValue(value);
        }

        private IEnumerator FadeInStatScreen()
        {
            Time.timeScale = 0;
            Color color = m_Curtain.color;
            m_Curtain.gameObject.SetActive(true);
            for (int i = 0; i < 50; i++)
            {
                float progress = (i + 1) / 50f;
                color.a = progress;
                m_Curtain.color = color;
                yield return null;
            }

            m_Container.gameObject.SetActive(true);

            for (int i = 0; i < 50; i++)
            {
                float progress = i / 50f;
                color.a = 1 - progress;
                m_Curtain.color = color;
                yield return null;
            }

            m_Curtain.gameObject.SetActive(false);
        }

        private IEnumerator FadeToMainMenu()
        {
            Color color = m_Curtain.color;
            m_Curtain.gameObject.SetActive(true);
            for (int i = 0; i < 50; i++)
            {
                float progress = (i + 1) / 50f;
                color.a = progress;
                m_Curtain.color = color;
                yield return null;
            }

            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}