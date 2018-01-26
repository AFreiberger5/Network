using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    public Rigidbody m_bulletPrefab;
    public Transform m_bulletSpawn;

    public int m_shotsPerBurst = 2;
    public float m_reloadTime = 1f;

    private int m_shotsLeft;
    private bool m_isReloading;
    public float m_dmgMod = 1f;
    public bool m_modBool = false;


	// Use this for initialization
	void Start ()
    {
        m_shotsLeft = m_shotsPerBurst;
        m_isReloading = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Shoot(float _dmgValue)
    {
        if (m_isReloading || m_bulletPrefab == null)
        {
            return;
        }

        CmdShoot(_dmgValue);

        m_shotsLeft--;
        if (m_shotsLeft <= 0)
        {
            StartCoroutine("Reload");
        }

    }

    [Command]
    private void CmdShoot(float _dmgValue)
    {
        CreateBullet(_dmgValue);
        RpcCreateBullet(_dmgValue);
    }

    [ClientRpc]
    void RpcCreateBullet(float _dmgValue)
    {
        if (!isServer)
        {
            CreateBullet(_dmgValue);
        }
    }

    void CreateBullet(float _dmgValue)
    {
        Bullet bullet = null;
        bullet = m_bulletPrefab.GetComponent<Bullet>();

        Rigidbody rbody = Instantiate(m_bulletPrefab, m_bulletSpawn.position, m_bulletSpawn.rotation) as Rigidbody;
        rbody.GetComponentInParent<Bullet>().DmgChange(_dmgValue);

        if (rbody != null)
        {
            rbody.velocity = bullet.m_speed * m_bulletSpawn.transform.forward;
            
        }
    }

    IEnumerator Reload()
    {
        m_shotsLeft = m_shotsPerBurst;
        m_isReloading = true;
        yield return new WaitForSeconds(m_reloadTime);
        m_isReloading = false;
    }
}
