using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataScript : NetworkBehaviour
{

    public List<PlayerController> m_players = new List<PlayerController>();

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    void AddPlayer(PlayerController _playerController)
    {
        //add player to list
        m_players.Add(_playerController);
        //send info to panel controls
        FindObjectOfType<PlayerPanel>().AddPlayers(m_players);
    }
}
