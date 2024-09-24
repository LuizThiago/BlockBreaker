using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [field: SerializeField] public Collider2D Collider { get; private set; }
    [Header("Events")]
    [SerializeField] private GameEvent _destructableDestroyedEvent;

    private int _hitPoints;
    private DestructibleDefiniton _definition;

    #region Public

    public void Init(int hitPoint, DestructibleDefiniton definition)
    {
        _definition = definition;
        _hitPoints = hitPoint;
        UpdateSprite();
    }

    public void TakeDamage(int damage = 1)
    {
        _hitPoints -= damage;
        if (_hitPoints <= 0)
        {
            Despawn();
            return;
        }

        UpdateSprite();
    }

    #endregion

    #region Private

    private void UpdateSprite()
    {
        if (_spriteRenderer == null) 
        {
            Debug.LogError($"[Destructible] Null ref for SpriteRender @ {gameObject.name}");
            return; 
        }

        if (_definition.TryGetSpriteForHitPoint(_hitPoints, out var sprite))
        {
            _spriteRenderer.sprite = sprite;
        }
    }

    private void Despawn()
    {
        _destructableDestroyedEvent.Raise(this, null);
        Destroy(gameObject);
    }

    #endregion
}
