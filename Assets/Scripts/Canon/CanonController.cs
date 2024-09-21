using UnityEngine;
using UnityEngine.InputSystem;

public class CanonController : MonoBehaviour
{
    [SerializeField] private Transform _canonTransform;
    [SerializeField] private InputActionReference _aimInputAction;
    [SerializeField] private CanonSettings _canonSettings;

    private Vector2 _mousePosition;

    #region Monobehaviour

    private void OnEnable()
    {
        if (_aimInputAction != null && _aimInputAction.action != null)
        {
            _aimInputAction.action.performed += OnAimInputPerformed;
            _aimInputAction.action.Enable();
        }
    }

    private void Update()
    {
        RotateCannonTowardsMouse();
    }

    private void OnDisable()
    {
        if (_aimInputAction != null && _aimInputAction.action != null)
        {
            _aimInputAction.action.performed -= OnAimInputPerformed;
            _aimInputAction.action.Disable();
        }
    }

    #endregion

    #region Private

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

        //We could clamp the angle for this angle range instead of returning,
        //but this results in a strange visual result...
        if (angle < _canonSettings.MinAngle || angle > _canonSettings.MaxAngle) { return; } 

        //Apply the rotation to the canon
        _canonTransform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    #endregion
}
