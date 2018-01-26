using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.UI;
using System.Net;

public class CustomNetworkManager : NetworkManager
{
    [Header("Custom Requirements")]
    public InputField m_NameInput;
    public Button m_HostButton;
    public Button m_ClientButton;
    public InputField m_HostInput;

    private string m_PlayerName = "Guest";

    private void Awake()
    {
        m_HostButton.interactable = false;
        m_ClientButton.interactable = false;

        m_NameInput.onValueChanged.AddListener(NameCheckFunction);
    }

    public override void OnClientConnect(NetworkConnection _conn)
    {
        base.OnClientConnect(_conn);
        Debug.Log("Client:" + _conn.address);// !!!
    }

    public override void OnServerConnect(NetworkConnection _conn)
    {
        base.OnServerConnect(_conn);
        Debug.Log("Server:" + _conn.address);// !!!
    }

    public void OnPlayerConnected(NetworkPlayer _player)
    {
        PlayerData playerData = new PlayerData()
        {
            name = m_PlayerName,// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            nplayer = _player
        };
    }

    public void NameCheckFunction(string _playerName)
    {
        m_HostButton.interactable = _playerName.Length > 0;
        m_ClientButton.interactable = _playerName.Length > 0;
    }

    public void HostOnClick()
    {
        m_PlayerName = m_NameInput.text;// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        Debug.Log(networkAddress);// !!!
        Debug.Log(m_NameInput.text);
        StartHost();
    }

    public void ClientOnClick()
    {
        m_PlayerName = m_NameInput.text;// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        networkAddress = IpChecker(m_HostInput.text);
        Debug.Log(networkAddress);// !!!
        StartClient();
    }

    private string IpChecker(string _ip)
    {
        IPAddress ip;
        if (IPAddress.TryParse(_ip, out ip))
            return ip.ToString();

        return "127.0.0.1";
    }
}

public struct PlayerData
{
    public string name;
    public NetworkPlayer nplayer;
}

//public class NetworkPlayerInfo : NetworkBehaviour
//{
//    public Player m_Player;

//    public void ReceivePlayer()
//    {
//        m_Player = new Player();
//    }
//}

//public sealed class Player
//{
//    public Player()
//    {
//        name = "";
//        tanga = "";
//    }

//    // name - string
//    // 
//    private readonly string name;
//    private readonly string tanga;
//}