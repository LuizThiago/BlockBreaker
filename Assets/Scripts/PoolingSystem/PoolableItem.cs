using UnityEngine;

public class PoolableItem : MonoBehaviour
{
    private IPool _pool;

    #region Public

    public void SetPool(IPool pool)
    {
        _pool = pool;
    }

    public void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Return(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}