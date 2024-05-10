using Core._Scripts.Networking.Client;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Runtime.Core._Scripts.Player
{
    public class PlayerServerName : NetworkBehaviour
    {
        public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                var userdata = HostSingleton.Instance.HostController.NetworkServer.GetUserDataByClientId(OwnerClientId);
                
                PlayerName.Value = userdata.UserName;
            }
        }
    }
}