using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PataRoad.Core.Character.Equipments
{
    public class GameObjectPool : MonoBehaviour
    {
        private readonly Dictionary<string, ObjectPool<GameObject>> _pools = new();

        public ObjectPool<GameObject> GetPool(string resourcePath, int initialSize, int maxSize)
        {
            if (_pools.TryGetValue(resourcePath, out var pool))
            {
                return pool;
            }

            void OnGet(GameObject obj)
            {
                obj.SetActive(true);
            }
            void OnRelease(GameObject obj)
            {
                obj.SetActive(false);
            }
            var gObject = Resources.Load<GameObject>(resourcePath);
            ObjectPool<GameObject> newPool = null;
            newPool = new ObjectPool<GameObject>(
                () => InstantiateObjectWithReleaseToPool(gObject, newPool),
                OnGet,
                OnRelease,
                null,//(obj) => Destroy(obj),
                false,
                initialSize,
                maxSize
                );

            InitializePool(newPool, initialSize);

            _pools.Add(resourcePath, newPool);

            return newPool;
        }

        private GameObject InstantiateObjectWithReleaseToPool(GameObject gObject, ObjectPool<GameObject> pool)
        {
            var obj = Instantiate(gObject, gameObject.transform);
            var rel = obj.AddComponent<ReleaseToPool>();
            rel.Pool = pool;
            return obj;
        }

        private void InitializePool(IObjectPool<GameObject> objectPool, int count)
        {
            var objects = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                objects[i] = objectPool.Get();
            }
            foreach (var obj in objects)
            {
                objectPool.Release(obj);
            }
        }
    }
}
