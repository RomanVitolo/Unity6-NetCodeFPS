using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core._Scripts.Networking.Client
{
    public class HostSingleton : MonoBehaviour
    {
        private static HostSingleton _instance;
        public HostController HostController { get; private set; }
        public static HostSingleton Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindFirstObjectByType<HostSingleton>();
                
                if (_instance == null)
                    return null;

                return _instance;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void CreateHost()
        {
            HostController = new HostController();
        }
    }
}