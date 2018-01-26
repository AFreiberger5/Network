using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : NetworkBehaviour
{
    Rigidbody m_rigidbody;
    Collider m_collider;
    public int m_speed = 20;
    public float m_damage = -5f;
    public float m_lifetime = 2f;
    List<ParticleSystem> m_allParticles;
    public List<string> m_collisionTags; //List gets filled in Editor for easier workflow
    public ParticleSystem m_exploFx;

    private NetworkInstanceId m_netId;
    // Use this for initialization
    void Start()
    {
        m_allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        StartCoroutine("SelfDestruct");
        Debug.Log(m_damage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DmgChange(float _dmgValue)
    {

        m_damage *= _dmgValue;

    }
    public void SetPlayerReference(NetworkInstanceId _netID)
    {
        m_netId = _netID;
        Debug.Log(m_netId + " thats the id");
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(m_lifetime);
        Explode();
    }

    private void Explode()
    {
        Debug.Log(m_damage + "on explo");
        m_collider.enabled = false;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.Sleep();

        foreach (ParticleSystem p in m_allParticles)
        {
            p.Stop();
        }
        if (m_exploFx != null)
        {
            m_exploFx.transform.parent = null;
            m_exploFx.Play();
        }

        // if (isServer)
        //{
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = false;
        }

        Destroy(gameObject);
        // }
    }

    void CheckCollisions(Collision _col)
    {

        if (m_collisionTags.Contains(_col.collider.tag))
        {

            //Find Player that shot the bullet if target was it
            PlayerController m_playerID = FindObjectsOfType<PlayerController>().Where(o => o.netId == m_netId).FirstOrDefault();
            FindObjectsOfType<PlayerController>().ToList().ForEach(o => Debug.Log(o.m_dmgMod + "dmmmg"));
            //Add points; todo: points depending on enemytype hit(- when player got hit?)
            m_playerID.GetComponent<PlayerHealth>().GetScorePoints(5);



            Debug.Log("Hit");
            Explode();
            Enemy_Stats_N_Stuff ESNS = _col.gameObject.GetComponentInParent<Enemy_Stats_N_Stuff>();

            // Instant One-Hit!
            ESNS.CalculateDamage(-5);
        }
    }
    private void OnCollisionEnter(Collision _col)
    {
        CheckCollisions(_col);
    }

    public void OnDestroy() // durch das unparenten verbleibt das Partikelsystem sonst in der Szene, Unitys Garbagecollector muss es löschen sobald das Gameobject zerstört wurde
    {
        if (m_exploFx != null)
            Destroy(m_exploFx.gameObject, 1f);
    }
}
