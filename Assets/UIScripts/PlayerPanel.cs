using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class PlayerPanel : NetworkBehaviour
{
    public List<PlayerPanelEntry> m_players = new List<PlayerPanelEntry>();
    public List<PlayerController> m_controllers = new List<PlayerController>();

    public GameObject m_EntryPrefab;

    // Use this for initialization
    void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnPlayerConnected(NetworkPlayer player)
    {
      
        string placeholder = "Player";
       // PlayerPanelEntry ppe = new PlayerPanelEntry(placeholder, player.);
    }
    [ServerCallback]
    public void AddPlayers(List<PlayerController> _list)
    {
        //clear lists because this function gets called everytime a player connects
        m_players.Clear();
        m_controllers.Clear();

        m_controllers = _list;
        foreach (PlayerController pc in m_controllers)
        {
            PlayerPanelEntry ppe = new PlayerPanelEntry("placeholder", pc);
            m_players.Add(ppe);
        }

        GameObject go;
        foreach (PlayerPanelEntry ppe in m_players)
        {
            //script is on parent, parent has allignment group so it will automatically moved into the correct place
            
            go = Instantiate(m_EntryPrefab, transform) as GameObject;
            go.transform.GetChild(0).GetComponent<Text>().text = ppe.m_Name;
            go.transform.GetChild(1).GetComponent<Text>().text = ppe.m_Score.ToString();
            //get player health from the playerhealth script on the object the playercontroller is on
            go.transform.GetChild(2).GetComponent<Text>().text = ppe.m_Player.GetComponent<PlayerHealth>().m_currentHealth.ToString();
            NetworkServer.Spawn(go);
            
        }

    }
       
}

public class PlayerPanelEntry
{
    public string m_Name;
    public PlayerController m_Player;
    public int m_Score;
    public float m_Health;
    public Image m_HPBar;

    public PlayerPanelEntry(string _name, PlayerController _player)
    {
        //set base parameters to have valid
        m_Name = _name;
        m_Player = _player;
        m_Score = 0;
        m_Health = 100;
        
    }
}
