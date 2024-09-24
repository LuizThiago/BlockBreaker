using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StagesController : MonoBehaviour
{
    [SerializeField] private StageList _stages;
    [SerializeField] private Destructible _destructiblePrefab;
    [SerializeField] private Transform _destructiblesContainer;
    [Header("Events")]
    [SerializeField] private GameEvent _hitEvent;
    [SerializeField] private GameEvent _destructibleDestroyedEvent;
    [SerializeField] private GameEvent _gameEndEvent;

    private readonly Dictionary<Collider2D, Destructible> _destructibles = new();

    #region Properties
    public string CurrentStageName => _stages.CurrentStageName;

    #endregion

    #region Monobehavour

    private void OnEnable()
    {
        _destructibleDestroyedEvent.RegisterResponse(OnDestructibleDestroyed);
        _hitEvent.RegisterResponse(OnHitEvent);
    }

    private void OnDisable()
    {
        _destructibleDestroyedEvent.UnRegisterResponse(OnDestructibleDestroyed);
        _hitEvent.UnRegisterResponse(OnHitEvent);
    }

    #endregion

    #region Public

    public void BuildStage(Action onFinish)
    {
        ClearStage();

        StartCoroutine(ProcessStageBuild(onFinish));
    }

    #endregion

    #region Private 

    private Vector2 GetStartPosition(StageDefinition stage)
    {
        float totalWidth = (stage.GridWidth - 1) * stage.Spacing;
        float totalHeight = (stage.GridHeight - 1) * stage.Spacing;

        Vector2 startPosition = new(
            _destructiblesContainer.position.x - (totalWidth / 2),
            _destructiblesContainer.position.y - (totalHeight / 2)
        );
        
        return startPosition;
    }
    private Vector2 GetSpawnPosition(StageDefinition stage, Vector2 startPosition, int x, int y)
    {
        return new Vector2(
            startPosition.x + (x * stage.Spacing),
            startPosition.y + (y * stage.Spacing)
        );
    }

    private void ClearStage()
    {
        if (_destructibles.Count == 0) { return; }

        var inGameDestructibles = _destructibles.Values.ToList();

        for (int i = inGameDestructibles.Count - 1; i >= 0; i--)
        {
            Destroy(inGameDestructibles[i].gameObject);
        }

        _destructibles.Clear();
    }

    private void OnHitEvent(Component _, object arg)
    {
        if (arg is Collider2D collider && _destructibles.ContainsKey(collider))
        {
            _destructibles[collider].TakeDamage();
        }
    }

    private void OnDestructibleDestroyed(Component sender, object _)
    {
        if (sender is Destructible destructible && _destructibles.ContainsKey(destructible.Collider))
        {
            _destructibles.Remove(destructible.Collider);
            CheckIfStageFinished();
        }
    }

    private void CheckIfStageFinished()
    {
        if (_destructibles.Count == 0)
        {
            if (_stages.TryGetNextStage(out var _))
            {
                _gameEndEvent.Raise(this, _stages.CurrentStageName);
                return;
            }

            _stages.ResetList();
            _gameEndEvent.Raise(this, null);
        }
    }

    private IEnumerator ProcessStageBuild(Action onFinish)
    {
        var currentStage = _stages.CurrentStage;
        Vector2 startPosition = GetStartPosition(currentStage);

        for (int x = 0; x < currentStage.GridWidth; x++)
        {
            for (int y = 0; y < currentStage.GridHeight; y++)
            {
                if (currentStage.Grid[x, y] > 0)
                {
                    Destructible destructible = SpawnDestructible(currentStage, startPosition, x, y);
                    InitDestructible(currentStage, x, y, destructible);

                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        onFinish?.Invoke();
    }

    private Destructible SpawnDestructible(StageDefinition currentStage, Vector2 startPosition, int x, int y)
    {
        Vector2 spawnPosition = GetSpawnPosition(currentStage, startPosition, x, y);
        var destructible = Instantiate(_destructiblePrefab, spawnPosition, Quaternion.identity, _destructiblesContainer.transform);
        return destructible;
    }

    private void InitDestructible(StageDefinition currentStage, int x, int y, Destructible destructible)
    {
        _destructibles.Add(destructible.Collider, destructible);
        destructible.gameObject.name = $"{_destructiblePrefab.name} {_destructibles.Count}";
        destructible.Init(currentStage.Grid[x, y], currentStage.DestructibleDefinition);
    }

    #endregion
}
