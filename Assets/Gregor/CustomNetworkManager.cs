using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;

public class CustomNetworkManager : NetworkManager
{
    [Header("Custom Requirements")]
    public InputField m_NameInput;
    public Button m_HostButton;
    public Button m_ClientButton;
    public InputField m_HostInput;

    private string m_PlayerName;
    private DataSyncFest m_Fest;

    private void Awake()
    {
        m_Fest = FindObjectOfType<DataSyncFest>();

        m_HostButton.interactable = false;
        m_ClientButton.interactable = false;

        m_NameInput.characterLimit = 10;
        m_NameInput.onValueChanged.AddListener(NameCheckFunction);
    }

    public override void OnClientConnect(NetworkConnection _conn)
    {
        base.OnClientConnect(_conn);
        // evtl. änderungen hier
    }
    
    // erstellt bei verbindungsaufbau relevante spielerdaten
    public override void OnServerConnect(NetworkConnection _conn)
    {
        base.OnServerConnect(_conn);

        PlayerData playerInfo = new PlayerData()
        {
            name = m_PlayerName,
            hostid = _conn.hostId
        };
        m_Fest.m_PlayerData.Add(playerInfo);

        //Debug.Log("hostid: " + playerInfo.hostid);
        //Debug.Log("playername: " + playerInfo.name);
        //Debug.Log("listcount: " + m_Fest.m_PlayerData.Count);
    }

    // um als host / client zu starten ist ein name mit mindestens einem buchstaben nötig
    public void NameCheckFunction(string _playerName)
    {
        m_HostButton.interactable = _playerName.Length > 0;
        m_ClientButton.interactable = _playerName.Length > 0;
    }

    // dummy funktion für den button
    public void HostOnClick()
    {
        m_PlayerName = m_NameInput.text;

        StartHost();
    }

    // dummy funktion für den button
    public void ClientOnClick()
    {
        m_PlayerName = m_NameInput.text;

        networkAddress = IpChecker(m_HostInput.text);

        StartClient();
    }

    private string IpChecker(string _ip)
    {
        IPAddress ip;
        if (IPAddress.TryParse(_ip, out ip))
            return ip.ToString();

        // bei fehlerhaftem input wird die localhost adresse zurückgegeben
        return "127.0.0.1";
    }
}