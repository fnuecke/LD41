using UnityEngine;

namespace MightyPirates
{
    public struct PooledObjectReference
    {
        private readonly GameObject m_GameObject;
        private readonly PooledObject m_PooledObject;
        private readonly int m_Version;

        public PooledObjectReference(GameObject gameObject)
        {
            m_GameObject = gameObject;
            m_PooledObject = gameObject != null ? gameObject.GetComponent<PooledObject>() : null;
            m_Version = m_PooledObject != null ? m_PooledObject.Version : 0;
        }

        public GameObject Value => m_PooledObject == null ? m_GameObject : m_PooledObject.Version == m_Version ? m_PooledObject.gameObject : null;

        public void Free()
        {
            if (m_PooledObject != null && m_PooledObject.Version == m_Version)
            {
                m_PooledObject.Free();
            }
        }
    }
}