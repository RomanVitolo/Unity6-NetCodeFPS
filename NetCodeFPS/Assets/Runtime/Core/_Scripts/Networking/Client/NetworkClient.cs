using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Core._Scripts.Networking.Client
{
    public class NetworkClient : IDisposable
    {                        
        private readonly NetworkManager _networkManager;

        private const string const_MenuSceneName = "Menu"; 

        public NetworkClient(NetworkManager networkManager)
        {
            _networkManager = networkManager;

            _networkManager.OnClientDisconnectCallback += OnClientDisconnect;          
        }                                                            
                                   
        private void OnClientDisconnect(ulong clientId)
        {
            if (clientId != 0 && clientId != _networkManager.LocalClientId) return;

            if (SceneManager.GetActiveScene().name != const_MenuSceneName) SceneManager.LoadScene(const_MenuSceneName);
            
            if(_networkManager.IsConnectedClient) _networkManager.Shutdown();
        }

        public void Dispose()
        {
            if (_networkManager != null)  
                _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;          
        }
    }
}