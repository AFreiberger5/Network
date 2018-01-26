using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DmgZone : NetworkBehaviour
{
    private Rigidbody m_EnemyRigidbody;
    
	// Use this for initialization
	void Awake ()
	{
        m_EnemyRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    [ServerCallback]

    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Enemy")
        {
            //m_EnemyRigidbody.velocity= Vector3.zero;
        }
    }
}
