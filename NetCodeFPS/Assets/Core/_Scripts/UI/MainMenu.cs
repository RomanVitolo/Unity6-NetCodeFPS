using Core._Scripts.Networking.Client;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
      [SerializeField] private TMP_InputField _joinCodeField;
    
      public async void StartHost()
      {
          HostSingleton.Instance.CreateHost();
          await HostSingleton.Instance.HostController.StartHostAsync();
      }

      public async void StartClient()
      {
          await ClientSingleton.Instance.ClientController.StartClientAsync(_joinCodeField.text);
      }
}
