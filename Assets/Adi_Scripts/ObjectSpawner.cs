using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSpawner : NetworkBehaviour
{


    public GameObject DmgOrb;
    private Vector3 spawpos;

    // Use this for initialization
    [ServerCallback]
    void Start()
    {
        if (isServer)
        {
            //GameObject wird immer nach gewisser zeit mit gewisser tickrate
            InvokeRepeating("RpcOrbSpawner", 10, 15);
        }
    }

    [ClientRpc]

    void RpcOrbSpawner()
    {

        spawpos.x = Random.Range(-5, 5);
        spawpos.y = 1;
        spawpos.z = Random.Range(-5, 5);

        GameObject go = Instantiate(DmgOrb, spawpos, Quaternion.identity);
        NetworkServer.Spawn(go);
    }
}
