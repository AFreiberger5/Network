using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DmgOrbScript :  NetworkBehaviour
{

    public List<string> m_collisionTags;
    private Bullet m_AddBulletDmg;
    private int m_damage = -5;
    private float m_damagetimer;


    private bool DmgBoost;

    void OnCollisionEnter(Collision _col)
    {
        CheckCollisions(_col);
    }
    void CheckCollisions(Collision _col)
    {
        if (m_collisionTags.Contains(_col.collider.tag))
        {

            Bullet m_AddBulletDmg = _col.gameObject.GetComponent<Bullet>();
            m_AddBulletDmg.m_damage = m_damage * 2;
            Destroy(gameObject);
           
        }
    }
}
