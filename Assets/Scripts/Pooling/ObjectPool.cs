using UnityEngine;

namespace MightyPirates
{
    [CreateAssetMenu]
    public sealed class ObjectPool : ScriptableObject
    {
        [SerializeField]
        private GameObject m_Prefab;

        private Transform m_Pool;

        private Transform GetPool()
        {
            if (m_Pool == null)
            {
                GameObject pool = new GameObject(m_Prefab.name);
                DontDestroyOnLoad(pool);
                m_Pool = pool.transform;
            }

            return m_Pool;
        }

        public GameObject Get()
        {
            return Get(Vector3.zero, Quaternion.identity);
        }

        public GameObject Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Transform pool = GetPool();

            if (pool.childCount == 0)
            {
                GameObject instance = Instantiate(m_Prefab, position, rotation, parent);
                DontDestroyOnLoad(instance);
                instance.GetOrAddComponent<PooledObject>().Pool = this;
                instance.SetActive(true);
                return instance;
            }

            Transform child = pool.GetChild(pool.childCount - 1);
            child.SetParent(parent, false);
            child.SetPositionAndRotation(position, rotation);
            child.gameObject.SetActive(true);
            return child.gameObject;
        }

        public void Free(GameObject instance)
        {
            PooledObject pooledObject = instance.GetComponent<PooledObject>();
#if DEBUG
            Debug.Assert(pooledObject.Pool == this);
#endif
            instance.SetActive(false);
            instance.transform.SetParent(GetPool());
            pooledObject.Version++;
        }
    }
}