using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>: MonoBehaviour where T : MonoBehaviour {
    public T spawnObjectPrefab;

    [SerializeField] private int _poolDepth;
    [SerializeField] private bool _canGrow = true;

    private List<T> _pool = new List<T>();

    private void Awake() {
        for (int i = 0; i < _poolDepth; i++) {
            var pooledObject = Instantiate(spawnObjectPrefab);
            pooledObject.gameObject.SetActive(false);
            _pool.Add(pooledObject);
            pooledObject.transform.parent = gameObject.transform;
        }
    }

    public T GetAvailableObject() {
        for (int i = 0; i < _pool.Count; i++) {
            if (!_pool[i].gameObject.activeInHierarchy)
                return _pool[i];
        }

        if (_canGrow == true) {
            var pooledObject = Instantiate(spawnObjectPrefab);
            pooledObject.gameObject.SetActive(false);
            _pool.Add(pooledObject);

            return pooledObject;
        }
        else {
            Debug.LogError("�� ������� ������� ����");
            return null;
        }
    }
}
