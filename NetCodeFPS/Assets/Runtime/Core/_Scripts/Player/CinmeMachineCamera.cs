using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CinmeMachineCamera : NetworkBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private int _ownerPriority = 15;
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)  
        {
            _camera.Priority = _ownerPriority;
        }
    }
}
