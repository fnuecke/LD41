using System.Collections;
using UnityEngine;

namespace MightyPirates
{
    public sealed class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private float m_SpawnInterval;

        [SerializeField]
        private int m_SpawnMin;

        [SerializeField]
        private int m_SpawnMax;

        [SerializeField]
        private float m_SpawnRadius;

        private Coroutine m_Coroutine;

        private void OnEnable()
        {
            m_Coroutine = StartCoroutine(SpawnPeriodically());
        }

        private void OnDisable()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_SpawnRadius);
        }

        private IEnumerator SpawnPeriodically()
        {
            for (;;)
            {
                yield return new WaitForSeconds(m_SpawnInterval);
                SpawnNow();
            }
        }

        private void SpawnNow()
        {
            int spawnCount = Random.Range(m_SpawnMin, m_SpawnMax + 1);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 relativePosition = Random.insideUnitCircle * m_SpawnRadius;
                Vector3 position = transform.position + relativePosition;
                ObjectPool.Get(m_Prefab, position, Quaternion.AngleAxis(Random.value * Mathf.PI * 2, Vector3.forward));
            }
        }
    }
}