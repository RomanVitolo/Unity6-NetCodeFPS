using System;
using Runtime.Utilities;        

namespace Core._Scripts.Networking.Client
{
    public class HostSingleton : GenericBaseSingleton<HostSingleton>
    {   
        public HostController HostController { get; private set; }        
        
        private void Start() => DontDestroyOnLoad(gameObject);    

        public void CreateHost() => HostController = new HostController();
                    
        private void OnDestroy() => HostController?.Dispose();    
    }
}