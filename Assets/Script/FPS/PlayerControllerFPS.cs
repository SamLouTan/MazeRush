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
    [SerializeField] private float bulletSpeed = 100f;

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
        HandleFire();
    }

    private void LateUpdate()
    {
        CamMouvements();
    }
    

   /* public void ChangeMouseSensitivity(float newSensitivity)
    {
        Sensitivity = newSensitivity;
        //if(onMouseSensitivityChange != null) onMouseSensitivityChange.Invoke(newSensitivity);
        PlayerPrefs.SetFloat("MouseSensitivity", newSensitivity);
        //save mouse sensitivity in PlayerPrefs with other settings
    }*/
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

    private void HandleFire()
    {
        if (_inputManager.Fire)
        {
            if (!isFireBeingHeld)
            {
                isFireBeingHeld = true;
                Shoot();
            }
        }
        else
            isFireBeingHeld = false;
    }
    
    private void Shoot()
    { 
        Transform cameratransform = Camera.transform;
        // Define a ray starting from the camera position and going forward
        Ray ray = new Ray(cameratransform.position, cameratransform.forward);

        // Create a RaycastHit variable to store information about what the ray hits
        RaycastHit hit;
        // rotate Bulletspwanpoint to camera rotation
        bulletSpawnPoint.rotation = Camera.rotation;
        Vector3 bulletSpawnPointPosition = bulletSpawnPoint.position;
    
        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            // Get the point where the ray hit
            Vector3 targetPoint = hit.point;

            // Calculate the direction from bulletSpawnPoint to the target point
            Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;

            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab,bulletSpawnPointPosition, Quaternion.identity);
            bullet.transform.localRotation = Quaternion.LookRotation(Camera.right); 
            // Set the velocity of the bullet to be in the calculated direction
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
        else
        {
            Vector3 targetPoint = cameratransform.position + cameratransform.forward * 1000f;
            Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;

            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPointPosition, Quaternion.identity);
            bullet.transform.localRotation = Quaternion.LookRotation(Camera.right); 
            // Set the velocity of the bullet to be in the calculated direction
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
    }
}