using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    public sealed class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private float m_SpawnInterval = 10;

        [SerializeField]
        private int m_SpawnMin = 1;

        [SerializeField]
        private int m_SpawnMax = 1;

        [SerializeField]
        private float m_SpawnRadius = 1;

        [SerializeField]
        private int m_MaxAlive = 10;

        private Coroutine m_Coroutine;
        private readonly LinkedList<PooledObjectReference> m_LiveChildren = new LinkedList<PooledObjectReference>();

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
            if (m_Prefab == null)
            {
                return;
            }

            int liveCount = 0;
            LinkedListNode<PooledObjectReference> node = m_LiveChildren.First;
            while (node != null)
            {
                GameObject pooledObject = node.Value.Value;
                if (pooledObject != null)
                {
                    liveCount++;
                    node = node.Next;
                }
                else
                {
                    LinkedListNode<PooledObjectReference> next = node.Next;
                    m_LiveChildren.Remove(node);
                    node = next;
                }
            }

            int spawnCount = Mathf.Min(Random.Range(m_SpawnMin, m_SpawnMax + 1), m_MaxAlive - liveCount);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 relativePosition = Random.insideUnitCircle * m_SpawnRadius;
                Vector3 position = transform.position + relativePosition;
                GameObject instance = ObjectPool.Get(m_Prefab, position, Quaternion.AngleAxis(Random.value * Mathf.PI * 2, Vector3.forward));
                m_LiveChildren.AddLast(new PooledObjectReference(instance));
            }
        }
    }
}