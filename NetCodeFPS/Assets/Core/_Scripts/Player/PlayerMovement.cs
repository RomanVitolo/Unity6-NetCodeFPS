using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Camera c_Camera;

    private const float f_Sens = 4f;
    private const float f_Gravity = 9.81f;
    private const float f_RotationSpeed = 7f;
    private float f_VerticalVelocity;

    private Vector2 v_previousMovement;
    private Vector2 v_previousRotation;

    private Vector3 v_movementDirection = Vector3.zero;

    private void Awake()
    {
        if(_controller == null) _controller = GetComponent<CharacterController>();
        if(c_Camera == null) c_Camera = GetComponentInChildren<Camera>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        _inputReader.MoveEvent += HandleMove;
        _inputReader.LookEvent += LookRotation;

    }

    private void LookRotation(Vector2 rotationInput)
    {
        v_previousRotation = rotationInput;
    }

    private void HandleMove(Vector2 movementInput)
    {
        v_previousMovement = movementInput;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        
        _inputReader.MoveEvent -= HandleMove;
        _inputReader.LookEvent -= LookRotation;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!IsOwner) return;

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        v_movementDirection = new Vector3(v_previousMovement.x, 0f, v_previousMovement.y);
        ApplyGravity();
        
        if (v_movementDirection.magnitude > 0)
            _controller.Move(v_movementDirection * (f_Sens * Time.deltaTime));
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded == false)
        {
            f_VerticalVelocity -= f_Gravity * Time.deltaTime;
            v_movementDirection.y = f_VerticalVelocity;
        }
        else
            f_VerticalVelocity = -0.5f;
    }

    private float rotationY;
    private float rotationX;
    private void LateUpdate()
    {
        if (!IsOwner) return;
        CameraRotation();
    }

    private void CameraRotation()
    {
        rotationX -= v_previousRotation.y;
        rotationX = Mathf.Clamp(rotationX, -20f, 20f);

        rotationY = v_previousRotation.x;

        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * rotationY);
    }
}
