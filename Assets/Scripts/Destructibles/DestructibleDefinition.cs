using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/DestructibleDefiniton", fileName = "new DestructibleDefiniton")]
public class DestructibleDefiniton : ScriptableObject
{
    [field: SerializeField] public int HitPoints { get; private set; } = 1;
    [SerializeField] private Sprite[] _sprites;

    public bool TryGetSpriteForHitPoint(int hitPoint, out Sprite sprite)
    {
        var index = hitPoint - 1;
        if (index >= 0 && index < _sprites.Length)
        {
            sprite = _sprites[index];
            return true;
        }

        if (_sprites.Length > 0)
        {
            sprite = _sprites[0];
            return true;
        }

        sprite = null;
        return false;
    }
}
