using UnityEngine;

namespace MightyPirates
{
    public sealed class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Prefab;

        public GameObject Get()
        {
            return Get(Vector3.zero, Quaternion.identity);
        }

        public GameObject Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (transform.childCount == 0)
            {
                GameObject instance = Instantiate(m_Prefab, position, rotation, parent);
                instance.GetOrAddComponent<PooledObject>().Pool = this;
                return instance;
            }

            Transform child = transform.GetChild(transform.childCount - 1);
            child.SetParent(parent, false);
            child.SetPositionAndRotation(position, rotation);
            return child.gameObject;
        }

        public void Free(GameObject instance)
        {
#if DEBUG
            Debug.Assert(instance.GetComponent<PooledObject>()?.Pool == this);
#endif
            instance.transform.SetParent(transform);
        }
    }
}