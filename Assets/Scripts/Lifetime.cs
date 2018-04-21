﻿using UnityEngine;

namespace MightyPirates
{
    [DisallowMultipleComponent]
    public sealed class Lifetime : MonoBehaviour
    {
        [SerializeField]
        private float m_Lifetime;

        private float m_TimeCreated;

        private void OnEnable()
        {
            m_TimeCreated = Time.time;
        }

        private void Update()
        {
            if (Time.time - m_TimeCreated > m_Lifetime)
            {
                this.FreeGameObject();
            }
        }
    }
}