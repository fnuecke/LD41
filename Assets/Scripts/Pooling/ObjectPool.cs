using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    public static class ObjectPool
    {
        private static readonly Dictionary<GameObject, Transform> Pools = new Dictionary<GameObject, Transform>();

        public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab == null)
                return null;

            Transform pool = GetPool(prefab);
            if (pool == null) // application exiting -> destroyed this frame
                return null;
            if (pool.childCount == 0)
            {
                GameObject instance = Object.Instantiate(prefab, position, rotation, parent);
                instance.GetOrAddComponent<PooledObject>().Prefab = prefab;
                instance.SetActive(true);
                return instance;
            }

            Transform child = pool.GetChild(pool.childCount - 1);
            child.SetParent(parent, false);
            child.SetPositionAndRotation(position, rotation);
            child.gameObject.SetActive(true);
            return child.gameObject;
        }

        public static void Free(PooledObject instance)
        {
            PooledObject pooledObject = instance.GetComponent<PooledObject>();
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(GetPool(pooledObject.Prefab), false);
            pooledObject.Version++;
        }

        private static Transform GetPool(GameObject prefab)
        {
            Transform pool;
            if (!Pools.TryGetValue(prefab, out pool))
            {
                GameObject gameObject = new GameObject(prefab.name);
                pool = gameObject.transform;
                Pools.Add(prefab, pool);
            }
            return pool;
        }
    }
}