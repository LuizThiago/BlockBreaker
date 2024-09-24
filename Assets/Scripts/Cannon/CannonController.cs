using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [SerializeField] private Transform _canonTransform;
    [SerializeField] private Transform _shootSpawnPoint;
    [SerializeField] private InputActionReference _aimInputAction;
    [SerializeField] private InputActionReference _shootInputAction;
    [SerializeField] private CannonSettings _canonSettings;
    [Header("Events")]
    [SerializeField] private GameEvent _gameStartEvent;
    [SerializeField] private GameEvent _gameEndEvent;
    [SerializeField] private GameEvent _shotInputEvent;

    private Vector2 _mousePosition;

    #region Monobehaviour

    private void OnEnable()
    {
        _gameStartEvent.RegisterResponse(OnGameStartEvent);
        _gameEndEvent.RegisterResponse(OnGameEndEvent);
    }

    private void Update()
    {
        if (!GameManager.IsRunning) { return; }

        RotateCannonTowardsMouse();
    }

    private void OnDisable()
    {
        _gameStartEvent.UnRegisterResponse(OnGameStartEvent);
        _gameEndEvent.UnRegisterResponse(OnGameEndEvent);
    }

    #endregion

    #region Private

    private void OnGameStartEvent(Component sender, object arg)
    {
        EnableInputs();
    }

    private void OnGameEndEvent(Component sender, object arg)
    {
        DisableInputs();
    }

    private void OnAimInputPerformed(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }

    private void RotateCannonTowardsMouse()
    {
        //Calculate the angle for the canon to look at the mouse position
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        Vector3 direction = worldMousePosition - _canonTransform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        if (angle < _canonSettings.MinAngle || angle > _canonSettings.MaxAngle) { return; } 

        //Apply the rotation to the canon
        _canonTransform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void OnShootInputPerformed(InputAction.CallbackContext context)
    {
        _shotInputEvent.Raise(this, new Vector2[] { _canonTransform.up, _shootSpawnPoint.position });
    }

    private void EnableInputs()
    {
        if (_aimInputAction != null && _aimInputAction.action != null)
        {
            _aimInputAction.action.performed += OnAimInputPerformed;
            _aimInputAction.action.Enable();
        }

        if (_shootInputAction != null && _shootInputAction.action != null)
        {
            _shootInputAction.action.performed += OnShootInputPerformed;
            _shootInputAction.action.Enable();
        }
    }

    private void DisableInputs()
    {
        if (_aimInputAction != null && _aimInputAction.action != null)
        {
            _aimInputAction.action.performed -= OnAimInputPerformed;
            _aimInputAction.action.Disable();
        }

        if (_shootInputAction != null && _shootInputAction.action != null)
        {
            _shootInputAction.action.performed -= OnShootInputPerformed;
            _shootInputAction.action.Disable();
        }
    }

    #endregion
}
