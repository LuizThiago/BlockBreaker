using UnityEngine;

[CreateAssetMenu(menuName = "Settings/CannonSettings", fileName = "new CannonSettings")]
public class CannonSettings : ScriptableObject
{
    [field: SerializeField] public float MinAngle { get; private set; } = 0f;
    [field: SerializeField] public float MaxAngle { get; private set; } = 180f;
}