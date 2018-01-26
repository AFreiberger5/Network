using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OrbPickups : NetworkBehaviour
{

    public List<string> m_collisionTags;
    private int m_damage = 5;


    private void OnTriggerEnter(Collider _col)
    {
        CheckCollisions(_col);
    }

    void CheckCollisions(Collider _col)
    {
        if (m_collisionTags.Contains(_col.gameObject.tag))
        {
            
            PlayerHealth playerHealth = _col.gameObject.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(m_damage);
                Destroy(gameObject);
               
            }
        }
    }
}
