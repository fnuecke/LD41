using UnityEngine;

namespace MightyPirates
{
    public sealed class PooledObject : MonoBehaviour
    {
        public ObjectPool Pool { get; set; }

        public int Version { get; set; }

        public void Free()
        {
            Pool.Free(gameObject);
        }
    }
}