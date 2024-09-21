using UnityEngine;

[CreateAssetMenu(menuName = "Settings/CanonSettings", fileName = "new CanonSettings")]
public class CanonSettings : ScriptableObject
{
    [field: SerializeField] public float MinAngle { get; private set; } = 0f;
    [field: SerializeField] public float MaxAngle { get; private set; } = 180f;
}