using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private Camera _cameraMain;
    [SerializeField] private CinemachineOrbitalFollow _cinemachineOrbitalFollow;
    [SerializeField] private Transform _cameraTarget;
    
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _orbitSensitivity = 0.5f;
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector2 _scrollWheelInput;
    private bool _middleClick;
    
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

    public void OnMiddleClick(InputValue value)
    {
        _middleClick = value.isPressed;
    }

    private void Update()
    {
        UpdateOrbit(Time.unscaledDeltaTime);
        UpdateMovement(Time.unscaledDeltaTime);
    }

    private void UpdateOrbit(float unscaledDeltaTime)
    {
        var orbitInput = _lookInput * (_middleClick ? 1f : 0f);
        orbitInput.y *= _orbitSensitivity;
        
        _cinemachineOrbitalFollow.HorizontalAxis.Value += orbitInput.x;
        _cinemachineOrbitalFollow.VerticalAxis.Value -= orbitInput.y;
        
        
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
        
        var motion =targetVelocity * unscaledDeltaTime;
        
        _cameraTarget.position += forward * motion.z + right * motion.x;
    }
}
