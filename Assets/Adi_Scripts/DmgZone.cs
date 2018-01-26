using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DmgZone : NetworkBehaviour
{
    private int m_EnmyDmg;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Enmy")
        {
            //Mach dem Gegener Schaden
        }
    }
}
