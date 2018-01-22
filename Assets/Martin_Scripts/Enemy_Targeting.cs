using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Targeting : NetworkBehaviour
{
    public GameObject[] mpu_Players;
    Enemy_Navigaton mpi_Enav;
    

    [ServerCallback]
    private void OnCollisionEnter(Collision _col)
    {
        mpi_Enav = GetComponentInParent<Enemy_Navigaton>();

        if (_col.gameObject.tag == "Player")
        {
            foreach(GameObject GO in mpu_Players)
            {
                if (GO == _col.gameObject)
                {
                    if (mpi_Enav.mpu_Target != null)
                    {
                        mpi_Enav.mpu_Target = GO.transform;
                    }
                }
            }
        }
    }
}
