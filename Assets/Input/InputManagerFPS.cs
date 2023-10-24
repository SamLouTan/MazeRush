using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerFPS : MonoBehaviour
{
    private PlayerInputFPS playerInputFPS;
    private PlayerInputFPS.OnFootActions onFoot;
    private PlayerMotorFPS motor;
    private PlayerLookFPS look;
    // Start is called before the first frame update
    void Awake()
    {
        playerInputFPS = new PlayerInputFPS();
        onFoot = playerInputFPS.OnFoot;
        motor = GetComponent<PlayerMotorFPS>();
        onFoot.Jump.performed += ctx => motor.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tell the playermotor to move using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
