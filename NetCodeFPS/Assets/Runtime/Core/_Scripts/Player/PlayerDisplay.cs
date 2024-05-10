using System;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Core._Scripts.Player
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerServerName _playerServerName;
        [SerializeField] private TMP_Text _playerNameText;
        
        private void Start()
        {
            HandlePlayerNameChanged(string.Empty, _playerServerName.PlayerName.Value);
            
            _playerServerName.PlayerName.OnValueChanged += HandlePlayerNameChanged;
        }

        private void HandlePlayerNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
        {
            _playerNameText.text = newName.ToString();
        }

        private void OnDestroy()
        {
            _playerServerName.PlayerName.OnValueChanged -= HandlePlayerNameChanged; 
        }
    }
}