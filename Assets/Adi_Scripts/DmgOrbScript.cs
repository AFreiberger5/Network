using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DmgOrbScript : NetworkBehaviour
{

    public List<string> m_collisionTags;
    private PlayerController playerController;



    [ServerCallback]
    private void OnTriggerEnter(Collider _col)
    {
        CheckCollisions(_col);
    }

    

    void CheckCollisions(Collider _col)
    {
        if (m_collisionTags.Contains(_col.gameObject.tag))
        {

            PlayerController playerController = _col.gameObject.GetComponentInParent<PlayerController>();
            if(playerController != null)
            {
                playerController.m_dmgMod *= 2;
            }

                Destroy(gameObject);
        }
    }
}
