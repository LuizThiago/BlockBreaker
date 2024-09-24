using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Console Log Settings", fileName = "new ConsoleLogSettings")]
public class ConsoleLogSettings : ScriptableObject
{
    [field: SerializeField] public int MaxMessages { get; private set; }
    [field: SerializeField] public LayerMask WallLayer { get; private set; }
    [field: SerializeField] public bool LogWallHits { get; private set; } = false;
    [SerializeField] private Color _hitLogColor;
    [SerializeField] private Color _shotLogColor;
    [SerializeField] private Color _destroyLogColor;

    public string HitLogColor { get; private set; }
    public string ShotLogColor { get; private set; }
    public string DestroyLogColor { get; private set; }

    #region Monobehaviour

    private void OnValidate()
    {
        HitLogColor = ColorToHex(_hitLogColor);
        ShotLogColor = ColorToHex(_shotLogColor);
        DestroyLogColor = ColorToHex(_destroyLogColor);
    }

    #endregion

    #region Private

    private string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int a = Mathf.RoundToInt(color.a * 255);

        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    #endregion
}