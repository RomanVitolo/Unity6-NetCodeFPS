using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharacterController _controller;

    [SerializeField] private Animator _playerAnimator;

    private const float f_Sens = 1.5f;
    private const float f_Gravity = 9.81f;
    private const float f_RotationSpeed = 10f;
    private const float f_smoothness = 35f;
    
    private float f_VerticalVelocity;

    private Vector2 v_previousMovement;
    private Vector2 v_previousRotation;

    private Vector3 v_movementDirection = Vector3.zero;   
    private Vector3 v_CurrentMovement;
    private static readonly int Speed = Animator.StringToHash("_speed");

    private void Awake()
    {
        if(_controller == null) _controller = GetComponent<CharacterController>();     
        if(_playerAnimator == null) _playerAnimator = GetComponent<Animator>();     
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        _inputReader.MoveEvent += HandleMove;  
        _inputReader.LookEvent += HandleLookRotation;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;                
    }       
 
    private void HandleLookRotation(Vector2 rotationInput)
    {
        v_previousRotation = rotationInput;
    }

    private void HandleMove(Vector2 movementInput)
    {
       v_previousMovement = movementInput;     
    }                               

    void Update()
    {
        if (!IsOwner) return;

        ApplyMovement();   
    }   
    
    private void ApplyMovement()
    {   
        v_movementDirection = new Vector3(v_previousMovement.x, 0f, v_previousMovement.y);     
        var v_worldDirection = transform.TransformDirection(v_movementDirection);  
         
        v_worldDirection.Normalize();

        v_CurrentMovement.x = v_worldDirection.x * f_Sens;     
        v_CurrentMovement.z = v_worldDirection.z * f_Sens;  
        
        _playerAnimator.SetFloat(Speed, Mathf.Abs(v_CurrentMovement.z));         
        
        ApplyGravity(); 

        if (v_movementDirection.magnitude > 0)  
            _controller.Move(v_CurrentMovement * Time.deltaTime);   
                
        ApplyRotation();            
    }    

    private void ApplyGravity()
    {
        if (_controller.isGrounded == false)
        {
            f_VerticalVelocity -= f_Gravity * Time.deltaTime;
            v_CurrentMovement.y = f_VerticalVelocity;
        }
        else
            f_VerticalVelocity = -.5f;
    }     

    private void ApplyRotation()
    {
        float mouseX = v_previousRotation.x * f_RotationSpeed * Time.deltaTime;
        float mouseY = v_previousRotation.y * f_RotationSpeed * Time.deltaTime;               

        transform.Rotate(Vector3.up * mouseX);  

        float desiredXRotation = transform.eulerAngles.x - mouseY;  

        desiredXRotation = Mathf.Clamp(desiredXRotation, -0.5f, 15f);   
        
        Quaternion desiredRotation = Quaternion.Euler(desiredXRotation, transform.eulerAngles.y, 0);     
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, f_smoothness * Time.deltaTime); 
    }
    
    
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        
        _inputReader.MoveEvent -= HandleMove;
        
        _inputReader.LookEvent -= HandleLookRotation;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
}
