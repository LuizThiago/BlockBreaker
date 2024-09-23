using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour, IPool where T : PoolableItem
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _initialSize = 10;

    private readonly Queue<T> _availableObjects = new();

    #region Monobehaviour

    private void Awake()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            T newObj = CreateNewItem();
            newObj.gameObject.SetActive(false);
            _availableObjects.Enqueue(newObj);
        }
    }

    #endregion

    #region Public

    public T GetItem(Vector3 position, Quaternion rotation)
    {
        T poolableItem;

        if (_availableObjects.Count > 0)
        {
            poolableItem = _availableObjects.Dequeue();
            poolableItem.transform.position = position;
            poolableItem.transform.rotation = rotation;
        }
        else
        {
            poolableItem = CreateNewItem();
        }

        poolableItem.gameObject.SetActive(true);
        return poolableItem;
    }

    public void Return(PoolableItem item)
    {
        item.gameObject.SetActive(false);
        item.transform.SetParent(transform);
        _availableObjects.Enqueue(item as T);
    }

    #endregion

    #region Private

    private T CreateNewItem()
    {
        T newObj = Instantiate(_prefab, transform);
        newObj.SetPool(this);
        return newObj;
    }

    #endregion
}
