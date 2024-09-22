using System.Collections.Generic;
using UnityEngine;

public class StagesController : MonoBehaviour
{
    [SerializeField] private StageList _stages;
    [SerializeField] private Destructible _destructiblePrefab;
    [SerializeField] private Transform _destructiblesContainer;
    [Header("Events")]
    [SerializeField] private GameEvent _destructibleDestroyedEvent;
    [SerializeField] private GameEvent _allDestructibleDestroyedEvent;

    private readonly List<Destructible> _destructibles = new();

    #region Monobehavour

    private void OnEnable()
    {
        _destructibleDestroyedEvent.RegisterResponse(OnDestructibleDestroyed);
    }

    private void Start()
    {
        BuildStage();
    }

    private void OnDisable()
    {
        _destructibleDestroyedEvent.UnRegisterResponse(OnDestructibleDestroyed);
    }

    #endregion

    #region Private 

    private void BuildStage()
    {
        ClearStage();

        var currentStage = _stages.CurrentStage;
        Vector2 startPosition = GetStartPosition(currentStage);

        for (int x = 0; x < currentStage.GridWidth; x++)
        {
            for (int y = 0; y < currentStage.GridHeight; y++)
            {
                if (currentStage.Grid[x, y])
                {
                    Vector2 spawnPosition = GetSpawnPosition(currentStage, startPosition, x, y);

                    var destructible = Instantiate(_destructiblePrefab, spawnPosition, Quaternion.identity, _destructiblesContainer.transform);
                    _destructibles.Add(destructible);
                }
            }
        }
    }

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

        for (int i = _destructibles.Count - 1; i >= 0; i--)
        {
            Destroy(_destructibles[i].gameObject);
        }

        _destructibles.Clear();
    }

    private void OnDestructibleDestroyed(Component sender, object _)
    {
        TryRemoveDestructible(sender);
    }

    private void TryRemoveDestructible(Component sender)
    {
        if (sender is Destructible destructible && _destructibles.Contains(destructible))
        {
            _destructibles.Remove(destructible);
            CheckIfStageFinished();
        }
    }

    private void CheckIfStageFinished()
    {
        if (_destructibles.Count == 0)
        {
            _allDestructibleDestroyedEvent.Raise(this, null);
        }
    }

    #endregion
}
