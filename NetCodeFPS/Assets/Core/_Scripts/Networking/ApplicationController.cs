using System.Threading.Tasks;
using Core._Scripts.Networking.Client;
using UnityEngine;

namespace Core._Scripts.Networking
{
    public class ApplicationController : MonoBehaviour
    {
        [SerializeField] private ClientSingleton _clientPrefab;
        [SerializeField] private HostSingleton _hostPrefab;
        
        private async void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
        }

        private async Task LaunchInMode(bool isDedicatedServer)
        {
            if (isDedicatedServer)
            {
                
            }
            else
            {
                HostSingleton hostSingleton = Instantiate(_hostPrefab);
                _hostPrefab.CreateHost();
                
                ClientSingleton clientSingleton = Instantiate(_clientPrefab);
                bool authenticated = await clientSingleton.CreateClient();

                if (authenticated)
                    clientSingleton.ClientController.GoToMenu();
            }
        }
    }
}