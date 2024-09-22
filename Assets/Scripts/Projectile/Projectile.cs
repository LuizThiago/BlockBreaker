using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask _collidableLayer;
    [SerializeField] private GameEvent _hitEvent;

    private Transform _transform;
    private Vector3 _direction;
    private float _speed;
    private float _lifetime;

    #region Properties

    public bool ShouldDestroy { get; private set; }

    #endregion

    #region Monobehaviours

    private void Awake()
    {
        _transform = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsInLayerMask(collision.gameObject)) { return; }

        if (TryProcessCollision(collision))
        {
            TriggerCollision(collision.gameObject);
        }
    }
    #endregion

    #region Public

    public void Init(Vector3 initialDirection, float speed, float lifeTime)
    {
        _direction = initialDirection;
        _speed = speed;
        _lifetime = lifeTime;
    }

    public void Move(float deltaTime)
    {
        _transform.position += _speed * deltaTime * _direction.normalized;
    }

    public void UpdateLifetime(float deltaTime)
    {
        _lifetime -= deltaTime;
        ShouldDestroy = _lifetime <= 0;
    }

    #endregion

    #region Private

    private bool IsInLayerMask(GameObject obj) => (_collidableLayer.value & (1 << obj.layer)) != 0;

    private bool TryProcessCollision(Collision2D collision)
    {
        Vector3 delta = CalculateCollisionDelta(collision.transform);
        return TryInvertDirection(delta);
    }

    private Vector3 CalculateCollisionDelta(Transform colliderTransform)
    {
        Vector3 delta = (transform.position - colliderTransform.position);
        Vector3 scaleFactor = GetScaleFactor(colliderTransform);
        delta.Scale(scaleFactor);

        return delta;
    }

    private Vector3 GetScaleFactor(Transform colliderTransform)
    {
        Vector3 colliderScale = colliderTransform.localScale;

        if (colliderScale.x > colliderScale.y)
        {
            return new Vector3(0.5f, 1f, 1f); //Collider is wider than it is tall
        }
        else if (colliderScale.y > colliderScale.x)
        {
            return new Vector3(1f, 0.5f, 1f); //Collider is taller than it is wide
        }
        else
        {
            return new Vector3(1f, 1f, 1f); //Collider is square
        }
    }

    private bool TryInvertDirection(Vector3 delta)
    {
        if (IsHorizontalCollision(delta))
        {
            if (Mathf.Sign(-_direction.x) == Mathf.Sign(delta.x))
            {
                _direction.x = -_direction.x;
                return true;
            }
        }
        else
        {
            if (Mathf.Sign(-_direction.y) == Mathf.Sign(delta.y))
            {
                _direction.y = -_direction.y;
                return true;
            }
        }

        return false;
    }

    private bool IsHorizontalCollision(Vector3 delta) => Mathf.Abs(delta.x) >= Mathf.Abs(delta.y);

    private void TriggerCollision(GameObject destructible)
    {
        _hitEvent.Raise(this, destructible);
    }

    #endregion
}
