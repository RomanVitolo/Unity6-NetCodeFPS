using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core._Scripts.Networking.Client
{
    public class ClientSingleton : MonoBehaviour
    {
        private static ClientSingleton _instance;

        public ClientController ClientController { get; private set; }
        
        public static ClientSingleton Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindFirstObjectByType<ClientSingleton>();
                
                if (_instance == null)
                    return null;

                return _instance;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async Task<bool> CreateClient()
        {
            ClientController = new ClientController();

            return await ClientController.InitAsync();
        }
    }
}