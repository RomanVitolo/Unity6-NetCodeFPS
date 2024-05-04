using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "Input/Input Reader", fileName = "New Input")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent; 
    public event Action<Vector2> MoveEvent; 
    public event Action<Vector2> LookEvent; 
    
    private Controls _controls;
    
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        
        _controls.Player.Enable();
    }
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
            PrimaryFireEvent?.Invoke(true);
        else if(context.canceled)
            PrimaryFireEvent?.Invoke(false);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    public void OnRun(InputAction.CallbackContext context)
    {
       
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
       
    }

    private void OnDisable() => _controls.Player.Disable();
}
