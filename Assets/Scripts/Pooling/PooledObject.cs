using UnityEngine;

namespace MightyPirates
{
    public sealed class PooledObject : MonoBehaviour
    {
        public GameObject Prefab { get; set; }
        public int Version { get; set; }

        public void Free()
        {
            ObjectPool.Free(this);
        }
    }
}