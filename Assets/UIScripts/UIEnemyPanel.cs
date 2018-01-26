using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyPanel : MonoBehaviour
{
    public Text EnemyMessageBox;
    public string m_enemyMessage;
	// Use this for initialization
	void Start ()
    {
        m_enemyMessage = "";
        EnemyMessageBox = GetComponentInChildren<Text>();
        EnemyMessageBox.enabled = false;
	}
	
    //Called from Enemy scripts whenever player should be warned
	public void OnWaveSpawn()
    {
        m_enemyMessage = "NEXT WAVE IN <color=red>5</color> SECONDS";
        EnemyMessageBox.text = m_enemyMessage;
        EnemyMessageBox.enabled = true;
    }
}
