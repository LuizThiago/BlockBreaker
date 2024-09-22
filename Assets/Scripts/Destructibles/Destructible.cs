using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Header("Events")]
    [SerializeField] private GameEvent _hitEvent;
    [SerializeField] private GameEvent _destructableDestroyedEvent;

    private int _hitPoints;
    private DestructibleDefiniton _definition;

    #region Monobehaviours

    private void OnEnable()
    {
        _hitEvent.RegisterResponse(OnHit);
    }

    private void OnDisable()
    {
        _hitEvent.UnRegisterResponse(OnHit);
    }

    #endregion

    #region Public

    public void Init(int hitPoint, DestructibleDefiniton definition)
    {
        _definition = definition;
        _hitPoints = hitPoint;
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
    private void OnHit(Component sender, object arg)
    {
        if (arg is GameObject objectHit && objectHit == gameObject)
        {
            TakeDamage();
        }
    }

    private void TakeDamage(int damage = 1)
    {
        _hitPoints -= damage;
        if (_hitPoints <= 0)
        {
            Despawn();
            return;
        }

        UpdateSprite();
    }

    private void Despawn()
    {
        _destructableDestroyedEvent.Raise(this, null);
        Destroy(gameObject);
    }

    #endregion
}
