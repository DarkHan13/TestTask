using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private float gravity = -9.8f; 
    [SerializeField] private float jumpHeight = 1f;

    [Header("Camera Settings")] 
    [SerializeField] private Transform camT;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxPitchAngle = 89f;

    private CharacterController _character;
    private Vector3 _velocity;
    private float _yawAngle;

    private void Reset()
    {
        camT = Camera.main.transform;
    }

    void Start()
    {
        _character = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMoveInput();
        RotateCamera();
    }

    private void HandleMoveInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * moveSpeed;
        // _character.Move(move * (moveSpeed * Time.deltaTime));

        if (Input.GetKeyDown(KeyCode.Space) && _character.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        move.y = _velocity.y;
        _character.Move(move * Time.deltaTime);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _yawAngle -= mouseY;
        _yawAngle = Mathf.Clamp(_yawAngle, -maxPitchAngle, maxPitchAngle);

        camT.localRotation = Quaternion.Euler(_yawAngle, 0f, 0f);
    }


}
