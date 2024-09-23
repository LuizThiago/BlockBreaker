using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ProjectileSettings _projectileSettings;
    [SerializeField] private Transform _projectilesContainer;
    [SerializeField] private ProjectilesPool _projectilePool;
    [Header("Events")]
    [SerializeField] private GameEvent _gameEndEvent;
    [SerializeField] private GameEvent _shotEvent;

    private readonly List<Projectile> _projectiles = new();
    private readonly List<Projectile> _toDestroy = new();

    #region Properties

    private float ProjectileLifeTime => _projectileSettings.LifeTime;
    private float ProjectileSpeed => _projectileSettings.Speed;

    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        _gameEndEvent.RegisterResponse(OnGameEndEvent);
    }

    private void Update()
    {
        if (!GameManager.IsRunning) { return; }

        var deltaTime = Time.deltaTime;

        foreach (var projectile in _projectiles)
        {
            projectile.Move(deltaTime);
            projectile.UpdateLifetime(deltaTime);

            if (projectile.ShouldDestroy)
            {
                _toDestroy.Add(projectile);
            }
        }

        if (_toDestroy.Count > 0)
        {
            foreach (var projectileToDestroy in _toDestroy)
            {
                _projectiles.Remove(projectileToDestroy);
                projectileToDestroy.ReturnToPool();
            }

            _toDestroy.Clear();
        }
    }

    private void OnDisable()
    {
        _gameEndEvent.UnRegisterResponse(OnGameEndEvent);
    }

    #endregion

    #region Public

    public void SpawnProjectile(Vector3 direction, Vector3 spawnPoint)
    {
        if (!GameManager.IsRunning) { return; }

        var projectile = _projectilePool.GetItem(spawnPoint, Quaternion.identity);
        projectile.transform.SetParent(_projectilesContainer, false);
        projectile.transform.position = spawnPoint;
        projectile.Init(direction, ProjectileSpeed, ProjectileLifeTime);

        _projectiles.Add(projectile);

        _shotEvent.Raise(this, projectile);
    }

    #endregion

    #region Private

    private void OnGameEndEvent(Component sender, object _)
    {
        DestroyAllProjectiles();
    }

    private void DestroyAllProjectiles()
    {
        foreach (var projectileToDestroy in _toDestroy)
        {
            _projectiles.Remove(projectileToDestroy);
            projectileToDestroy.ReturnToPool();
        }

        _toDestroy.Clear();

        for (int i = _projectiles.Count - 1; i >= 0; i--)
        {
            _projectiles[i].ReturnToPool();
        }

        _projectiles.Clear();
    }

    #endregion
}
