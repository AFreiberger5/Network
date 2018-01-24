using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSpawner : NetworkBehaviour
{

    
    public GameObject Healthorb;
    private float spawndelay = 5f;
    private Vector3 spawpos;
    
	// Use this for initialization
    [ServerCallback]
	void Start ()
	{
        if(isServer)
	    OrbSpawner();
    }
	
	// Update is called once per frame
	void Update ()
	{
		
	}
    [ServerCallback]
    void OrbSpawner()
    {

        //spawndelay = -Time.deltaTime;
        //if (spawndelay <= 0)
        //{
        
        
            spawpos.x = 0;
            spawpos.y = 5;
            spawpos.z = 0;

            Instantiate(Healthorb, spawpos, Quaternion.identity);

        
            //spawndelay = 5f;

        //}
    }
}
