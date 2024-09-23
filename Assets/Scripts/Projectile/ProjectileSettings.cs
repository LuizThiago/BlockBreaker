using UnityEngine;

[CreateAssetMenu(menuName = "Settings/ProjectileSettings", fileName = "new ProjectileSettings")]
public class ProjectileSettings : ScriptableObject
{
    [field: SerializeField] public float LifeTime { get; private set; } = 5f;
    [field: SerializeField] public float Speed { get; private set; } = 5f;
}