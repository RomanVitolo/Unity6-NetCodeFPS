using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private Button _connectButton;
    [SerializeField] private int _minNameLength = 1;
    [SerializeField] private int _maxNameLength = 12;

    public const string const_playerNameKey = "PlayerName";
    
    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }                          
        
        _nameField.text = PlayerPrefs.GetString(const_playerNameKey, string.Empty);
        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        _connectButton.interactable =
            _nameField.text.Length >= _minNameLength &&
            _nameField.text.Length <= _maxNameLength;
    }

    public void Connect()
    {
        PlayerPrefs.SetString(const_playerNameKey, _nameField.text);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
