using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MightyPirates
{
    public sealed class MainMenu : MonoBehaviour
    {
        private const int CurtainFadeSteps = 50;

        [SerializeField]
        private Image m_Curtain;

        public void Play()
        {
            StartCoroutine(FadeInCurtain());
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            StartCoroutine(FadeOutCurtain());
        }

        private IEnumerator FadeOutCurtain()
        {
            Color color = m_Curtain.color;
            for (int i = 0; i < CurtainFadeSteps; i++)
            {
                float progress = i / (float) CurtainFadeSteps;
                color.a = 1 - progress;
                m_Curtain.color = color;
                yield return null;
            }
            m_Curtain.gameObject.SetActive(false);
        }

        private IEnumerator FadeInCurtain()
        {
            m_Curtain.gameObject.SetActive(true);
            Color color = m_Curtain.color;
            for (int i = 0; i < CurtainFadeSteps; i++)
            {
                float progress = (i + 1) / (float) CurtainFadeSteps;
                color.a = progress;
                m_Curtain.color = color;
                yield return null;
            }
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}