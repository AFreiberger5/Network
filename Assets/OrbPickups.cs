using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OrbPickups : NetworkBehaviour
{

    public List<string> m_collisionTags;
    private int m_damage = 5;
    void OnCollisionEnter(Collision _col)
    {
        CheckCollisions(_col);
    }
    void CheckCollisions(Collision _col)
    {
        if (m_collisionTags.Contains(_col.collider.tag))
        {
            
            PlayerHealth playerHealth = _col.gameObject.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(m_damage);
                Destroy(gameObject);
                //_col.gameObject.SetActive(false);
                // + gib dem Spieler + x-HP
            }
        }
    }
}
