using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CannonController _canonController;
    [SerializeField] private ProjectileController _projectileController;
    [Header("Events")]
    [SerializeField] private GameEvent _gameStartEvent;
    [SerializeField] private GameEvent _gameEndEvent;
    [SerializeField] private GameEvent _allDestructibleDestroyedEvent;

    #region Properties

    public static CannonController CanonController => Instance._canonController;
    public static ProjectileController ProjectileController => Instance._projectileController;
    public static bool IsRunning { get; private set; }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        Instance._allDestructibleDestroyedEvent.RegisterResponse(OnAllDestructibleDestroyed);
    }

    private void Start()
    {
        IsRunning = true;
        _gameStartEvent.Raise(this, null);
    }

    private void OnDestroy()
    {
        if (Instance != null)
        {
            Instance._allDestructibleDestroyedEvent.UnRegisterResponse(OnAllDestructibleDestroyed);
        }
    }

    #endregion

    #region Private

    private static void OnAllDestructibleDestroyed(Component sender, object _)
    {
        IsRunning = false;
        Instance._gameEndEvent.Raise(Instance, null);
    }

    #endregion
}
