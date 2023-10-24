using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class PlayerControllerFPS : MonoBehaviour
{
    //[System.Serializable]
    //public class MouseSensitivityChangeEvent : UnityEvent<float> { }
    
    //public MouseSensitivityChangeEvent onMouseSensitivityChange;
    
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = CONSTANTS.PLAYER_BOTTOM_LIMIT;
    [SerializeField] private float Bottomlimit = CONSTANTS.PLAYER_UPPER_LIMIT;
    [SerializeField] private float Sensitivity = CONSTANTS.MOUSE_SENSITIVITY;
    private Rigidbody _playerRigidbody;
  
    private InputManager _inputManager;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 1000f;

    //Acces aux HashID de Xvelocity et Yvelocity
    //Acces aux Animations

    private bool isFireBeingHeld;

    //Camera Variables
    private float _xRotation;


    //Player vitesse
    private const float _walkSpeed = 3f;
    private const float _runSpeed = 5f;

    private Vector3 startPosition;
    private Vector2 _currentVelocity;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            Sensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        }
    }

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();
        isFireBeingHeld = false;
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CamMouvements();
    }
    

    public void ChangeMouseSensitivity(float newSensitivity)
    {
        Sensitivity = newSensitivity;
        //if(onMouseSensitivityChange != null) onMouseSensitivityChange.Invoke(newSensitivity);
        PlayerPrefs.SetFloat("MouseSensitivity", newSensitivity);
        //save mouse sensitivity in PlayerPrefs with other settings
    }
    private void Move()
    {
        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        
        _playerRigidbody.position += (transform.right *_inputManager.Move.x * targetSpeed *Time.deltaTime);
        
        _playerRigidbody.position += (transform.forward *_inputManager.Move.y *targetSpeed *Time.deltaTime);
        
    }

    private void CamMouvements()
    {

        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;

        _xRotation -= Mouse_Y * Sensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, Bottomlimit);
        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerRigidbody.MoveRotation((_playerRigidbody.rotation.normalized) *
                                      Quaternion.Euler(0, Mouse_X * Sensitivity * Time.smoothDeltaTime, 0));
    }

}