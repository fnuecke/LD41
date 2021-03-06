﻿using UnityEngine;

namespace MightyPirates
{
    [DefaultExecutionOrder((int) ExectionOrders.PlayerCamera)]
    public sealed class PlayerCamera : MonoBehaviour
    {
        public static AudioSource GlobalAudioSource => s_Instance.m_AudioSource;

        private static PlayerCamera s_Instance;

        [SerializeField]
        private AnimationCurve m_ShakyCurve;

        [SerializeField]
        private float m_ShakeSpeed = 2;

        private AudioSource m_AudioSource;
        private PooledObjectReference m_Player;
        private float m_ShakeOffset = 1;

        private void OnEnable()
        {
            m_Player = new PooledObjectReference(GameObject.FindGameObjectWithTag("Player"));
            s_Instance = this;
            m_AudioSource = this.GetOrAddComponent<AudioSource>();
        }

        private void OnDisable()
        {
            s_Instance = null;
        }

        private void Update()
        {
            if (m_Player.Value == null)
                return;

            Vector2 lookDirection = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 playerPosition = m_Player.Value.transform.position;
            Vector2 playerLookVector = Vector2.ClampMagnitude(Vector2.Scale(lookDirection, new Vector2(1f / Screen.width, 1f / Screen.height)), 0.5f) * 2;
            Vector3 cameraPosition = playerPosition + playerLookVector;
            cameraPosition.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, cameraPosition, 5f * Time.deltaTime);

            if (m_ShakeOffset < 1)
            {
                m_ShakeOffset += Time.deltaTime * m_ShakeSpeed;
                float angle;
                if (m_ShakeOffset >= 1)
                {
                    angle = 0;
                }
                else
                {
                    angle = m_ShakyCurve.Evaluate(m_ShakeOffset);
                }
                Vector3 angles = transform.rotation.eulerAngles;
                angles.z = angle;
                transform.rotation = Quaternion.Euler(angles);
            }
        }

        [ContextMenu("Shake")]
        public void Shake()
        {
            m_ShakeOffset = 0;
        }
    }
}