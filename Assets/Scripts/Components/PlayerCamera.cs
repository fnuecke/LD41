﻿using UnityEngine;

namespace MightyPirates
{
    public sealed class PlayerCamera : MonoBehaviour
    {
        private PooledObjectReference m_Player;

        private void OnEnable()
        {
            m_Player = new PooledObjectReference(GameObject.FindGameObjectWithTag("Player"));
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

            transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.5f);
        }
    }
}