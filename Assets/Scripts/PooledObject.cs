using UnityEngine;

namespace MightyPirates
{
    public sealed class PooledObject : MonoBehaviour
    {
        private ObjectPool m_Pool;

        public void SetPool(ObjectPool pool)
        {
            m_Pool = pool;
        }

        public void Free()
        {
            m_Pool.Free(gameObject);
        }
    }
}