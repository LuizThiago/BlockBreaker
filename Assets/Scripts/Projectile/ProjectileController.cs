using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ProjectileSettings _projectileSettings;
    [SerializeField] private Transform _projectilesContainer;
    private readonly List<Projectile> _projectiles = new();
    private readonly List<Projectile> _toDestroy = new();

    #region Properties

    private Projectile ProjectilePrefab => _projectileSettings.ProjectilePrefab;
    private float ProjectileLifeTime => _projectileSettings.LifeTime;
    private float ProjectileSpeed => _projectileSettings.Speed;

    #endregion

    #region Monobehaviour

    private void Update()
    {
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
                Destroy(projectileToDestroy.gameObject);
            }

            _toDestroy.Clear();
        }
    }

    #endregion

    #region Public

    public void SpawnProjectile(Vector3 direction, Vector3 spawnPoint)
    {
        var projectile = Instantiate(ProjectilePrefab, spawnPoint, Quaternion.identity, _projectilesContainer);
        projectile.Init(direction, ProjectileSpeed, ProjectileLifeTime);

        _projectiles.Add(projectile);
    }

    #endregion
}
