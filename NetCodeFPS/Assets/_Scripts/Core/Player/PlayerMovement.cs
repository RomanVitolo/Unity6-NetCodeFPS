using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CharacterController _controller;

    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float rotationSpeed = 150f;

    private Vector2 _previousMovement;
    private Vector2 _previousRotation;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputReader.MoveEvent += HandleMove;
        _inputReader.LookEvent += LookRotation;

    }

    private void LookRotation(Vector2 rotationInput)
    {
        _previousRotation = rotationInput;
    }

    private void HandleMove(Vector2 movementInput)
    {
        _previousMovement = movementInput;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        
        _inputReader.MoveEvent -= HandleMove;
        _inputReader.LookEvent -= LookRotation;
    }


    void Update()
    {
        if (!IsOwner) return;

        var deltaX  = _previousMovement.x;
        var deltaZ = _previousMovement.y;

        _controller.Move(new Vector3(deltaX, 0f, deltaZ) * (movementSpeed * Time.deltaTime));
        
        var rotateY  = _previousRotation.x;

        transform.Rotate(Vector3.up, rotateY * rotationSpeed * Time.deltaTime, Space.World);
    }
}
