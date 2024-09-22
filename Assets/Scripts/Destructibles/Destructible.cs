using UnityEngine;

public class Destructible : MonoBehaviour, IGameEventListener
{
    [SerializeField] private DestructibleDefiniton _definition;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameEvent _hitEvent;

    private int _hitPoints;

    #region Monobehaviours

    private void OnEnable()
    {
        _hitEvent.RegisterListener(this);
    }

    private void Awake()
    {
        _hitPoints = _definition.HitPoints;
        UpdateSprite();
    }

    private void OnDisable()
    {
        _hitEvent.UnRegisterListener(this);
    }

    #endregion

    #region Protected

    void IGameEventListener.OnEventRaised(Component sender, object arg)
    {
        if (arg is GameObject objectHit && objectHit == gameObject)
        {
            TakeDamage();
        }
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
        Destroy(gameObject);
    }

    #endregion
}
