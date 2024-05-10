using System;
using System.Collections.Generic;
using Core._Scripts.Networking.Shared;
using Newtonsoft.Json;
using Unity.Netcode;     

public class NetworkServer : IDisposable
{
     private Dictionary<ulong, string> ClientIdToAuth = new Dictionary<ulong, string>();
     private Dictionary<string, UserData> AuthIdToUserData = new Dictionary<string, UserData>();
     
     private readonly NetworkManager _networkManager;    

     public NetworkServer(NetworkManager networkManager)
     {
          _networkManager = networkManager;

          _networkManager.ConnectionApprovalCallback += ApprovalCheck;
          networkManager.OnServerStarted += OnNetworkReady;
     }      

     private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, 
          NetworkManager.ConnectionApprovalResponse response)
     {
          var payload = System.Text.Encoding.UTF8.GetString(request.Payload);
          var userData = JsonConvert.DeserializeObject<UserData>(payload);

          ClientIdToAuth[request.ClientNetworkId] = userData.UserAuthId;
          AuthIdToUserData[userData.UserAuthId] = userData;

          response.Approved = true;
          response.CreatePlayerObject = true;
     }
     
     private void OnNetworkReady()
     {
          _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
     }

     private void OnClientDisconnect(ulong clientId)
     {
          if(ClientIdToAuth.TryGetValue(clientId, out string authId))
          {
               ClientIdToAuth.Remove(clientId);
               AuthIdToUserData.Remove(authId);
          }
     }

     public void Dispose()
     {
          if (_networkManager != null) return;

          _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
          _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
          _networkManager.OnServerStarted -= OnNetworkReady;

          if (_networkManager.IsListening)    
               _networkManager.Shutdown();           
     }
}
