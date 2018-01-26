using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.UI;

public class CustomNetworkHud : NetworkBehaviour
{
    public Button m_HostButton;
    public Button m_ClientButton;
    public Button m_QuitButton;

    public InputField m_NameInput;
    public InputField m_HostInput;

    private void Awake()
    {
        m_NameInput.onValueChanged.AddListener(NameCheckFunction);
    }

    public void NameCheckFunction(string _playerName)
    {
        m_NameInput.interactable = _playerName.Length > 0;
        Debug.Log(_playerName);
    }

    public void DummyHost()
    {
        
    }

    public void DummyClient()
    {

    }
}