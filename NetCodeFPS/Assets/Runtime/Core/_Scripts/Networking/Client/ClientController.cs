using System;
using System.Text;
using System.Threading.Tasks;
using Core._Scripts.Networking.Shared;
using Newtonsoft.Json;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core._Scripts.Networking.Client
{
    public class ClientController : IDisposable
    {
        private JoinAllocation _allocation;
        
        private const string MenuScene = "Menu";

        private NetworkClient _networkClient;
        
        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();

            _networkClient = new NetworkClient(NetworkManager.Singleton);

            AuthState authState = await AuthenticationWrapper.DoAuth();

            if (authState == AuthState.Authenticated) return true;

            return false;
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(MenuScene);
        }

        public async Task StartClientAsync(string joinCode)
        {
            try
            {
                _allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            
            RelayServerData relayServerData = new RelayServerData(_allocation, "dtls");
            transport.SetRelayServerData(relayServerData);

            UserData userData = new UserData
            {
                UserName = PlayerPrefs.GetString(NameSelector.const_playerNameKey, "Missing Name"),
                UserAuthId = AuthenticationService.Instance.PlayerId
            };
            string payload = JsonConvert.SerializeObject(userData);
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
            
            NetworkManager.Singleton.StartClient();
        }

        public void Dispose() => _networkClient?.Dispose();  
    }
}