using System;
using System.Collections;
using System.Collections.Generic;
using MoveCharactere.Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace MoveCharactere.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;

        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float Bottomlimit = 70f;
        [SerializeField] private float Sensitivity = 21.9f;
        [SerializeField] private float JumpFactor = 260f;
        [SerializeField] private float Dis2Ground = 0.8f;
        [SerializeField] private LayerMask GroundCheck;
        [SerializeField] private float AirResistance = 0.8f;
        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;
        
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 1000f;
        
        //Acces aux Animations
        private Animator _animator;
        private bool _grounded = false;
        private bool _hasAnimator;
        
        //Acces aux HashID de Xvelocity et Yvelocity
        private int _xValHash;
        private int _yValHash;
        private int _zValHash;
        private int _jumpHash;
        private int _groundedHash;
        private int _fallingHash;
        private int _crouchHash;
        private int _fireHash;

        private bool isFireBeingHeld;
        
        //Camera Variables
        private float _xRotation;
        
        
        //Player vitesse
        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;

        private Vector2 _currentVelocity;
        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
            isFireBeingHeld = false;

            _xValHash = Animator.StringToHash("X_Velocity");
            _yValHash = Animator.StringToHash("Y_Velocity");
            _zValHash = Animator.StringToHash("Z_Velocity");
            _jumpHash = Animator.StringToHash("Jump");
            _fallingHash = Animator.StringToHash("Falling");
            _groundedHash = Animator.StringToHash("Grounded");
            _crouchHash = Animator.StringToHash("Crouch");
            _fireHash = Animator.StringToHash("Fire");

        }

        private void FixedUpdate()
        {
            SampleGround();
            Move();
            HandleJump();
            HandleCrouch();
            HandleFire();
        }

        private void LateUpdate()
        {
            CamMouvements();
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if(_inputManager.Crouch) targetSpeed = 1.5f;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0;
            
            //A Supprimer si on veut deplacement dans l'air
            if (_grounded)
            {
                _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.deltaTime);
                _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.deltaTime);

                var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
                var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.y;
                
                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference,0,zVelDifference)),ForceMode.VelocityChange);   
            }
            else
            {
                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x * AirResistance ,0, _currentVelocity.y * AirResistance)), ForceMode.VelocityChange);
            }

            _animator.SetFloat(_xValHash,_currentVelocity.x);
            _animator.SetFloat(_yValHash,_currentVelocity.y);
        }

        private void CamMouvements()
        {
            if (!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * Sensitivity * Time.smoothDeltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, Bottomlimit);
            
            Camera.localRotation = Quaternion.Euler(_xRotation,0, 0);
            _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * Sensitivity * Time.smoothDeltaTime, 0));
        }

        private void HandleCrouch() => _animator.SetBool(_crouchHash, _inputManager.Crouch);
        private void HandleJump()
        {
            if(!_hasAnimator) return;
            if(!_inputManager.Jump) return;
            _animator.SetTrigger(_jumpHash);
        }

        public void JumpAddForce()
        { 
            _playerRigidbody.AddForce(-_playerRigidbody.velocity.y * Vector3.up , ForceMode.VelocityChange);
            _playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);  
            _animator.ResetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if (!_hasAnimator) return;

            RaycastHit hitInfo;
            if (Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out hitInfo, Dis2Ground + 0.1f,
                    GroundCheck))
            {
                //Collided with sth
                //Grounded
                _grounded = true;
                SetAnimationGrounding();
                return;
            }
            _grounded = false;
            _animator.SetFloat(_zValHash, _playerRigidbody.velocity.y);
            SetAnimationGrounding();
            //Falling
        }

        private void HandleFire()
        {
            if (!_hasAnimator) return;

            if (_inputManager.Fire)
            {
                if (!isFireBeingHeld)
                {
                    isFireBeingHeld = true;
                    _animator.SetBool(_fireHash, _inputManager.Fire);
                    Shoot();
                }
            }
            else
                isFireBeingHeld = false;
                _animator.SetBool(_fireHash, _inputManager.Fire);
        }

        private void SetAnimationGrounding()
        {
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundedHash, _grounded);
        }

        private void Shoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
    }
}
