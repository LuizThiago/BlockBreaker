using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float _startTimeDelay = 3f;
    [Header("References")]
    [SerializeField] private CannonController _canonController;
    [SerializeField] private ProjectileController _projectileController;
    [SerializeField] private StagesController _stagesController;
    [SerializeField] private InputActionReference _startGameInputAction;
    [Header("Events")]
    [SerializeField] private GameEvent _playerStartedEvent;
    [SerializeField] private GameEvent _gameStartEvent;
    [SerializeField] private GameEvent _gameEndEvent;

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
        Instance._gameEndEvent.RegisterResponse(OnGameEnd);
    }

    private void Start()
    {
        EnableInputs();
    }

    private void OnDestroy()
    {
        if (Instance != null)
        {
            Instance._gameEndEvent.UnRegisterResponse(OnGameEnd);
        }
    }

    #endregion

    #region Private

    private void EnableInputs()
    {
        if (_startGameInputAction != null && _startGameInputAction.action != null)
        {
            _startGameInputAction.action.performed += OnStartGameInputPerformed;
            _startGameInputAction.action.Enable();
        }
    }

    private void DisableInputs()
    {
        if (_startGameInputAction != null && _startGameInputAction.action != null)
        {
            _startGameInputAction.action.performed -= OnStartGameInputPerformed;
            _startGameInputAction.action.Disable();
        }
    }

    private void OnStartGameInputPerformed(InputAction.CallbackContext context)
    {
        StartGame();
    }

    private void StartGame()
    {
        _playerStartedEvent.Raise(this, _stagesController.CurrentStageName);
        StartCoroutine(PrepareStage());
    }

    private static void OnGameEnd(Component sender, object arg)
    {
        IsRunning = false;

        if (arg is null)
        {
            Instance.EnableInputs();
            return;
        }

        Instance.StartGame();
    }

    private IEnumerator PrepareStage()
    {
        DisableInputs();

        yield return new WaitForSeconds(_startTimeDelay);

        _stagesController.BuildStage();
        IsRunning = true;
        _gameStartEvent.Raise(this, null);
    }

    #endregion
}
