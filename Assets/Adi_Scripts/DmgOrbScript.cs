using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DmgOrbScript : NetworkBehaviour
{

    public List<string> m_collisionTags;
    private Bullet m_AddBulletDmg;




    private void OnTriggerEnter(Collider _col)
    {
        CheckCollisions(_col);
    }

    void CheckCollisions(Collider _col)
    {
        if (m_collisionTags.Contains(_col.gameObject.tag))
        {

            Bullet m_AddBulletDmg = _col.gameObject.GetComponent<Bullet>();
            Destroy(gameObject);

        }
    }
}
