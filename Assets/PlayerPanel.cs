using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPanel : NetworkBehaviour
{
    readonly List<PlayerPanelEntry> m_players = new List<PlayerPanelEntry>();

    public GameObject m_EntryPrefab;

    // Use this for initialization
    void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {
		
	}
	void AddPlayers()
    {

    }
       
}

class PlayerPanelEntry
{
    public string m_Name;
    public PlayerController m_Player;
    public int m_Score;


    public PlayerPanelEntry(string _name, PlayerController _player)
    {
        m_Name = _name;
        m_Player = _player;
        m_Score = 0;
    }
}
