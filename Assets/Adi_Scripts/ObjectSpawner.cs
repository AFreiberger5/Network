using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSpawner : NetworkBehaviour
{


    public GameObject Healthorb;
    //private float spawndelay = 5f;
    private Vector3 spawpos;

    // Use this for initialization
    [ServerCallback]
    void Start()
    {
        if (isServer)
        {
            //GameObject wird immer nach gewisser zeit mit gewisser tickrate
            InvokeRepeating("RpcOrbSpawner", 10, 20);

        }
    }

    [ClientRpc]

    void RpcOrbSpawner()
    {

        spawpos.x = Random.Range(-5, 5);
        spawpos.y = 1;
        spawpos.z = Random.Range(-5, 5);

        GameObject go = Instantiate(Healthorb, spawpos, Quaternion.identity);
        NetworkServer.Spawn(go);



    }
}
