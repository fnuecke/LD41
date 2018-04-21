using UnityEngine;

namespace MightyPirates
{
    public static class UnityObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component != null) return component;
            return gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.GetOrAddComponent<T>();
        }

        public static void Free(this GameObject gameObject)
        {
            PooledObject pooledObject = gameObject.GetComponent<PooledObject>();
            if (pooledObject != null) pooledObject.Free();
            else Object.Destroy(gameObject);
        }

        public static void FreeGameObject(this Component component)
        {
            component.gameObject.Free();
        }

        public static float GetRadius(this GameObject gameObject)
        {
            CircleCollider2D circleCollider = gameObject.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
                return circleCollider.radius;
            return 1f;
        }
    }
}