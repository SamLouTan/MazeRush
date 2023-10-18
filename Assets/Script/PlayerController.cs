using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float AnimBlendSpeed = 8.9f;

    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float Bottomlimit = 70f;
    [SerializeField] private float Sensitivity = 21.9f;
    private Rigidbody _playerRigidbody;
    private InputManager _inputManager;

    //Acces aux Animations
    private Animator _animator;
    private bool _hasAnimator;

    //Acces aux HashID de Xvelocity et Yvelocity
    private int _xValHash;
    private int _yValHash;

    //Camera Variables
    private float _xRotation;

    //Player vitesse
    private const float _walkSpeed = 2f;
    private const float _runSpeed = 6f;

    private Vector2 _currentVelocity;

    private void Awake()
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();

        _xValHash = Animator.StringToHash("X_Velocity");
        _yValHash = Animator.StringToHash("Y_Velocity");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CamMouvements();
    }

    private void Move()
    {
        if (!_hasAnimator) return;

        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if (_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed,
            AnimBlendSpeed * Time.deltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed,
            AnimBlendSpeed * Time.deltaTime);

        var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
        var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.y;

        _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)),
            ForceMode.VelocityChange);

        _animator.SetFloat(_xValHash, _currentVelocity.x);
        _animator.SetFloat(_yValHash, _currentVelocity.y);
    }

    private void CamMouvements()
    {
        if (!_hasAnimator) return;

        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        Camera.position = CameraRoot.position;

        _xRotation -= Mouse_Y * Sensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, Bottomlimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * Sensitivity * Time.smoothDeltaTime, 0));
    }
}