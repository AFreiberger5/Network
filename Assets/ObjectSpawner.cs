using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSpawner : NetworkBehaviour
{

    
    public GameObject Healthorb;
    private float spawndelay = 5f;
    private Vector3 spawpos = new Vector3(0, 5, 0);
    
	// Use this for initialization
	void Start ()
	{
	    OrbSpawner();
    }
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    void OrbSpawner()
    {
        spawndelay = -Time.deltaTime;
        if (spawndelay <= 0)
        {
            Instantiate(Healthorb, spawpos, Quaternion.identity);
            spawndelay = 5f;

        }
    }
}
