using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController3D : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float dashSpeed = 250f;
    [SerializeField] private float gravity = -12f;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController controller;

    private bool _hasToDash;

    private Vector3 _playerVelocity;
    private bool _isGrounded;

    // Update is called once per frame
    private void Update()
    {
        _isGrounded = controller.isGrounded;

        if (playerInput.actions["Dash"].WasPressedThisFrame() && _isGrounded)
        {
            _hasToDash = true;
        }
        
    }

    private void FixedUpdate()
    {
        var movementsInput = playerInput.actions["Movements"].ReadValue<Vector2>();
        var moveDirection = transform.forward * movementsInput.y + transform.right * movementsInput.x;
        
        var speed = moveSpeed;
        if (playerInput.actions["Sprint"].IsPressed())
        {
            speed = sprintSpeed;
        }

        _playerVelocity.y += gravity * Time.deltaTime;

        if (_hasToDash)
        {
            controller.Move(_playerVelocity * (dashSpeed * Time.deltaTime));
            _hasToDash = false;
        }
        else
        {
            _playerVelocity.x = moveDirection.x * speed;
            _playerVelocity.z = moveDirection.z * speed;
        }

        controller.Move(_playerVelocity * Time.deltaTime);

    }
}
