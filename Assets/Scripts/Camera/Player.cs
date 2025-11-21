using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Camera _cameraMain;

    [SerializeField] private CinemachineOrbitalFollow _cinemachineOrbitalFollow;
    [SerializeField] private Transform _cameraTarget;

    [Space(10)] 
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 20f;

    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _decceleration = 10f;
    [SerializeField] private float _zoomSmoothing = 0.5f;
    [SerializeField] private float _zoomSpeed = 0.5f;

    [SerializeField] private float _orbitSensitivity = 0.5f;
    [SerializeField] private float _orbitSmoothing = 5f;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector2 _scrollWheelInput;
    private Vector3 _velocity;
    private bool _middleDown;
    private bool _leftRotateDown;
    private bool _rightRotateDown;

    private float _currentZoom;
    private int _rotateInput;

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    public void OnScrollWheel(InputValue value)
    {
        _scrollWheelInput = value.Get<Vector2>();
    }

    public void OnRightRotate(InputValue value)
    {
        _rightRotateDown = value.isPressed;
        _rotateInput = value.isPressed ? -1 : (_leftRotateDown ? 1 : 0);
    }

    public void OnLeftRotate(InputValue value)
    {
        _leftRotateDown = value.isPressed;
        _rotateInput = value.isPressed ? 1 : (_rightRotateDown ? -1 : 0);
    }

    public void OnMiddleClick(InputValue value)
    {
        _middleDown = value.isPressed;
    }

    private void LateUpdate()
    {
        UpdateZoom(Time.unscaledDeltaTime);
        UpdateOrbit(Time.unscaledDeltaTime);
        UpdateMovement(Time.unscaledDeltaTime);
        UpdateRotation(Time.unscaledDeltaTime);
    }

    private void UpdateRotation(float unscaledDeltaTime)
    {
        if (_middleDown) return;

        InputAxis horizontalAxis = _cinemachineOrbitalFollow.HorizontalAxis;
        horizontalAxis.Value = Mathf.Lerp(horizontalAxis.Value, horizontalAxis.Value + _rotateInput, _orbitSmoothing * unscaledDeltaTime);

        _cinemachineOrbitalFollow.HorizontalAxis = horizontalAxis;
    }

    private void UpdateOrbit(float unscaledDeltaTime)
    {
        if (!_middleDown && _lookInput.sqrMagnitude < 0.01) return;

        var orbitInput = _lookInput * (_middleDown ? 1 : 0f);
        orbitInput *= _orbitSensitivity;

        InputAxis horizontalAxis = _cinemachineOrbitalFollow.HorizontalAxis;
        InputAxis verticalAxis = _cinemachineOrbitalFollow.VerticalAxis;

        horizontalAxis.Value = Mathf.Lerp(horizontalAxis.Value, horizontalAxis.Value + orbitInput.x, _orbitSmoothing * unscaledDeltaTime);
        verticalAxis.Value = Mathf.Lerp(verticalAxis.Value, verticalAxis.Value - orbitInput.y, _orbitSmoothing * unscaledDeltaTime);

        verticalAxis.Value = Mathf.Clamp(verticalAxis.Value, verticalAxis.Range.x, verticalAxis.Range.y);

        _cinemachineOrbitalFollow.HorizontalAxis = horizontalAxis;
        _cinemachineOrbitalFollow.VerticalAxis = verticalAxis;
    }

    private void UpdateMovement(float unscaledDeltaTime)
    {
        var forward = _cameraMain.transform.forward;
        forward.y = 0;
        forward.Normalize();

        var right = _cameraMain.transform.right;
        right.y = 0;
        right.Normalize();

        var targetVelocity = new Vector3(_moveInput.x, 0, _moveInput.y) * _moveSpeed;

        if (_moveInput.sqrMagnitude > 0.01f)
        {
            _velocity = Vector3.MoveTowards(_velocity, targetVelocity, _acceleration * unscaledDeltaTime);
        }
        else
        {
            _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _decceleration * unscaledDeltaTime);
        }

        var motion = _velocity * unscaledDeltaTime;

        _cameraTarget.position += forward * motion.z + right * motion.x;
    }

    private void UpdateZoom(float unscaledDeltaTime)
    {
        var radialAxis = _cinemachineOrbitalFollow.RadialAxis;

        var targetZoomSpeed = 0f;

        if (Mathf.Abs(_scrollWheelInput.y) >= 0.01f)
        {
            targetZoomSpeed = _zoomSpeed * _scrollWheelInput.y;
        }

        _currentZoom = Mathf.Lerp(_currentZoom, targetZoomSpeed, _zoomSmoothing * unscaledDeltaTime);
        radialAxis.Value -= _currentZoom;
        radialAxis.Value = Mathf.Clamp(radialAxis.Value, radialAxis.Range.x, radialAxis.Range.y);

        _cinemachineOrbitalFollow.RadialAxis.Value = radialAxis.Value;
    }
}