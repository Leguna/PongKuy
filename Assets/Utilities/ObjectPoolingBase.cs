using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class ObjectPoolingBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T prefab;

        private List<T> pool;

        public void Init(T newPrefab, int newPoolSize = 10)
        {
            prefab = newPrefab;
            pool = new List<T>(newPoolSize);
            for (var i = 0; i < newPoolSize; i++)
            {
                var newItem = Instantiate(prefab, transform);
                newItem.gameObject.SetActive(false);
                pool.Add(newItem);
            }
        }

        public T GetObject()
        {
            foreach (var item in pool.Where(item => !item.gameObject.activeInHierarchy))
            {
                item.gameObject.SetActive(true);
                return item;
            }

            var newItem = Instantiate(prefab, transform);
            pool.Add(newItem);
            newItem.gameObject.SetActive(true);

            return newItem;
        }

        public T ReturnObject(T item)
        {
            item.gameObject.SetActive(false);
            return item;
        }
    }
}