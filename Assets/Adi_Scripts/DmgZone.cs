using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DmgZone : NetworkBehaviour
{
    private Enemy_Stats_N_Stuff enemy_Stats_N_Stuff;

	// Use this for initialization
	void Awake ()
	{
        enemy_Stats_N_Stuff = GetComponent<Enemy_Stats_N_Stuff>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Enemy")
        {
            
        }
    }
}
