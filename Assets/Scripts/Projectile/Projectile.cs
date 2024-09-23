using UnityEngine;

public class Projectile : PoolableItem
{
    [SerializeField] private LayerMask _collidableLayer;
    [SerializeField] private LayerMask _wallLayer;
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
        var isCollidable = ContainsLayerMask(_collidableLayer, collision.gameObject);
        var isWall = ContainsLayerMask(_wallLayer, collision.gameObject);
        if (!isCollidable && !isWall) { return; }

        ProcessCollision(collision);
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
    private bool ContainsLayerMask(LayerMask layerMask, GameObject obj) => (layerMask.value & (1 << obj.layer)) != 0;

    private void ProcessCollision(Collision2D collision)
    {
        InvertDirection(collision);
        _hitEvent.Raise(this, collision.gameObject);
    }

    private void InvertDirection(Collision2D collision)
    {
        _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
    }

    #endregion
}
