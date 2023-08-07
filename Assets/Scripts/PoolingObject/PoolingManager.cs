using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System;

namespace PierreMizzi.Useful.PoolingObjects
{


    public class PoolingManager : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private PoolingChannel m_poolingChannel = null;

        [SerializeField]
        private Transform m_poolsContainer = null;

        private Dictionary<string, ObjectPool<GameObject>> m_objectPools =
            new Dictionary<string, ObjectPool<GameObject>>();

        #endregion

        #region Methods


        private void Awake()
        {
            Subscribe();
        }

        private void Start()
        {
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (m_poolingChannel != null)
            {
                m_poolingChannel.onCreatePool += CallbackCreateInPool;
                m_poolingChannel.onGetFromPool += CallbackGetFromPool;
                m_poolingChannel.onReleaseFromPool += CallbackReleaseFromPool;
            }
        }

        private void Unsubscribe()
        {
            if (m_poolingChannel != null)
            {
                m_poolingChannel.onCreatePool -= CallbackCreateInPool;
                m_poolingChannel.onGetFromPool -= CallbackGetFromPool;
                m_poolingChannel.onReleaseFromPool -= CallbackReleaseFromPool;
            }
        }

        private void CallbackReleaseFromPool(GameObject gameObject)
        {
            if (m_objectPools.ContainsKey(gameObject.name))
            {
                m_objectPools[gameObject.name].Release(gameObject);
            }
        }

        private GameObject CallbackGetFromPool(GameObject gameObject)
        {
            if (m_objectPools.ContainsKey(gameObject.name))
                return m_objectPools[gameObject.name].Get();
            else
                return null;
        }

        private void CallbackCreateInPool(PoolConfig config)
        {
            if (!m_objectPools.ContainsKey(config.prefab.name))
            {
                // Creates a container
                GameObject container = new GameObject();
                container.name = $"PoolContainer_{config.prefab.name}";
                container.transform.parent = m_poolsContainer;

                // Creates a special method for creating pool instances
                System.Func<GameObject> create = () =>
                {
                    GameObject gameObject = Instantiate(
                        config.prefab,
                        Vector3.zero,
                        Quaternion.identity,
                        container.transform
                    );
                    gameObject.name = config.prefab.name;
                    return gameObject;
                };

                // Create a new pool
                ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                    create,
                    GetFromPool,
                    ReleaseToPool,
                    DestroyFromPool,
                    true,
                    config.defaultSize,
                    config.maxSize
                );

                // Add the pool in the dictionnary
                m_objectPools.Add(config.prefab.name, pool);
            }
        }

        private void ReleaseToPool(GameObject poolObject)
        {
            poolObject.SetActive(false);
        }

        private void GetFromPool(GameObject poolObject)
        {
            poolObject.SetActive(true);
        }

        private void DestroyFromPool(GameObject poolObject)
        {
            Destroy(poolObject);
        }

        #endregion
    }
}
