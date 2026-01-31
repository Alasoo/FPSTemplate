using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
    public class Pool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T prefab;

        public static Pool<T> Instance { get; private set; }
        public Queue<T> pool = new Queue<T>();

        private void Awake()
        {
            // Asignación del Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            //FillPool();
        }

        /*
        //POR SI SE QUIERE INICIAR AL PRINCIPIO EN EL BASE
        private void FillPool()
        {
            for (int i = 0; i < initialAmount; i++)
                CreateAndEnqueue();
        }
        */

        private T CreateAndEnqueue()
        {
            T go = Instantiate(prefab);
            go.transform.SetParent(transform);
            go.gameObject.SetActive(false);

            T component = go.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError($"El prefab en {gameObject.name} no tiene el componente {typeof(T).Name}");
                Destroy(go);
                return null;
            }

            pool.Enqueue(component);
            return component;
        }

        public T Get()
        {
            T item;

            if (pool.Count > 0)
                item = pool.Dequeue();
            else
            {
                item = CreateAndEnqueue();
                if (pool.Count > 0) item = pool.Dequeue();
            }

            //item.gameObject.SetActive(true);
            return item;
        }

        public virtual void Return(T item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(transform);
            pool.Enqueue(item);
        }
    }
}
