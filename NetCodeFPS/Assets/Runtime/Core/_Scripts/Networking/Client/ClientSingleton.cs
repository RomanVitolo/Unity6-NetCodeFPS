using System;
using System.Threading.Tasks;
using Runtime.Utilities;       

namespace Core._Scripts.Networking.Client
{
    public class ClientSingleton : GenericBaseSingleton<ClientSingleton>
    {   

        public ClientController ClientController { get; private set; }     

        private void Start() => DontDestroyOnLoad(gameObject);       
        
        public async Task<bool> CreateClient()
        {
            ClientController = new ClientController();

            return await ClientController.InitAsync();
        }

        private void OnDestroy() => ClientController?.Dispose();  
    }
}