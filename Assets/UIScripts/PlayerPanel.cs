using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        GameObject go;

        foreach (PlayerPanelEntry ppe in m_players)
        {
            //script is on parent, parent has allignment group so it will automatically moved into the correct place
            
            go = Instantiate(m_EntryPrefab, transform) as GameObject;

            go.transform.GetChild(0).GetComponent<Text>().text = ppe.m_Name;
            go.transform.GetChild(1).GetComponent<Text>().text = ppe.m_Score.ToString();
            //todo: change 100 to players HP variable
            go.transform.GetChild(2).GetComponent<Text>().text = "100";
        }

    }
       
}

class PlayerPanelEntry
{
    public string m_Name;
    public PlayerController m_Player;
    public int m_Score;
    public float m_Health;

    public PlayerPanelEntry(string _name, PlayerController _player)
    {
        //set base parameters to have valid
        m_Name = _name;
        m_Player = _player;
        m_Score = 0;
        m_Health = 100;
    }
}
