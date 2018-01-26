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

    public void Shoot(float _dmgValue, NetworkInstanceId _netId)
    {
        if (m_isReloading || m_bulletPrefab == null)
        {
            return;
        }

        CmdShoot(_dmgValue, _netId);

        m_shotsLeft--;
        if (m_shotsLeft <= 0)
        {
            StartCoroutine("Reload");
        }

    }

    [Command]
    private void CmdShoot(float _dmgValue, NetworkInstanceId _netId)
    {
        CreateBullet(_dmgValue, _netId);
        RpcCreateBullet(_dmgValue, _netId);
    }

    [ClientRpc]
    void RpcCreateBullet(float _dmgValue, NetworkInstanceId _netId)
    {
        if (!isServer)
        {
            CreateBullet(_dmgValue, _netId);
        }
    }

    void CreateBullet(float _dmgValue, NetworkInstanceId _netId)
    {
        

        Bullet bullet = null;
        bullet = m_bulletPrefab.GetComponent<Bullet>();
        Debug.Log(_netId + " thats the id");
        Rigidbody rbody = Instantiate(m_bulletPrefab, m_bulletSpawn.position, m_bulletSpawn.rotation) as Rigidbody;
        rbody.GetComponentInParent<Bullet>().DmgChange(_dmgValue);
        rbody.GetComponentInParent<Bullet>().SetPlayerReference(_netId);


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
