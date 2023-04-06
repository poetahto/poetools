using UnityEngine;

namespace poetools.Core
{
    public class PooledPrefabFactory : IGameObjectFactory
    {
        private readonly ObjectPool<GameObject> _objectPool;
        private readonly GameObject _prefab;

        public PooledPrefabFactory(GameObject prefab)
        {
            _prefab = prefab;
            
            _objectPool = new ObjectPool<GameObject>(
                CreatePooledInstance, 
                actionOnGet:     pooledGameObject => pooledGameObject.SetActive(true), 
                actionOnRelease: pooledGameObject => pooledGameObject.SetActive(false), 
                actionOnDestroy: Release
            );
        }

        private GameObject CreatePooledInstance()
        {
            GameObject instance = Object.Instantiate(_prefab);
            instance.SetActive(false);
            return instance;
        }

        public GameObject Create()
        {
            return _objectPool.Get();
        }

        public void Release(GameObject instance)
        {
            _objectPool.Release(instance);
        }
    }
}