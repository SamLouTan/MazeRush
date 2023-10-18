using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace MoveCharactere.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput; 
        
        // read-only Variable pour savoir le status des inputs
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }
        public bool Jump { get; private set; }
        public bool Crouch { get; private set; }
        
        public bool Fire { get; private set; }
        
        
        //Acces a la Current InputActionMap et aux InputAction Individuellement
        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _runAction;
        private InputAction _jumpAction;
        private InputAction _crouchAction;
        private InputAction _fireAction;
        
        //Obtention de la InputMap et attache au inputAction
        private void Awake()
        {
            Hidecursor();
            _currentMap = PlayerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            _jumpAction = _currentMap.FindAction("Jump");
            _crouchAction = _currentMap.FindAction("Crouch");
            _fireAction = _currentMap.FindAction("Fire");
            
            //Start Fonction
            _moveAction.performed += OnMove;
            _lookAction.performed += OnLook;
            _runAction.performed += OnRun;
            _jumpAction.performed += OnJump;
            _crouchAction.performed += OnCrouch;
            _fireAction.performed += OnFire;
            
            //Stop Fonction
            _moveAction.canceled += OnMove;
            _lookAction.canceled += OnLook;
            _runAction.canceled += OnRun;
            _jumpAction.canceled += OnJump;
            _crouchAction.canceled += OnCrouch;
            _fireAction.canceled += OnFire;
        }

        private void Hidecursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Onmove Method
        private void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        
        //OnLook Method
        private void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }
        
        //OnRun Method
        private void OnRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
        
        private void OnJump(InputAction.CallbackContext context)
        { 
            Jump = context.ReadValueAsButton();
        }
        
        private void OnCrouch(InputAction.CallbackContext context)
        { 
            Crouch = context.ReadValueAsButton();
        }

        private void OnFire(InputAction.CallbackContext context)
        {
            Fire = context.ReadValueAsButton();
        }
        private void OnEnable()
        {
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
        }
    }
}