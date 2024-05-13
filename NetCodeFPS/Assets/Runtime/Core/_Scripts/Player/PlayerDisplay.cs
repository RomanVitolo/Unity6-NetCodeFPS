using System;
using TMPro;
using Unity.Cinemachine;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Core._Scripts.Player
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerServerName _playerServerName;
        [SerializeField] private TMP_Text _playerNameText;
        
        private Transform cameraTransform;              
        
        private void Start()
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;

            HandlePlayerNameChanged(string.Empty, _playerServerName.PlayerName.Value);
            
            _playerServerName.PlayerName.OnValueChanged += HandlePlayerNameChanged;
        }

        private void Update()
        {
            if (cameraTransform != null)
            {
                transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                    cameraTransform.rotation * Vector3.up);
            }
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