using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerSetup))]

public class PlayerController : NetworkBehaviour
{

    PlayerHealth m_pHealth;
    PlayerShoot m_pShoot;
    PlayerMotor m_pMotor;
    PlayerSetup m_pSetup;

    Vector3 m_originalPosition;
    NetworkStartPosition[] m_spawnPoints;

    public GameObject m_spawnFx;
    public float m_dmgMod = 1f;

    GameObject m_InfoPanel;
    private bool m_PanelActive;
    private NetworkInstanceId m_netID;

    // Use this for initialization
    void Start()
    {

        m_pHealth = GetComponent<PlayerHealth>();
        m_pShoot = GetComponent<PlayerShoot>();
        m_pMotor = GetComponent<PlayerMotor>();
        m_pSetup = GetComponent<PlayerSetup>();

        m_InfoPanel = GameObject.FindGameObjectWithTag("InfoPanel");
        m_netID = this.netId;

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        m_spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();
        m_originalPosition = transform.position;
        //on connect add playerdata to list to use it for ui
        FindObjectOfType<DataScript>().m_players.Add(this);

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || m_pHealth.m_isDead)
        {
            return;
        }

        Vector3 inputDirection = GetInput();
        m_pMotor.MovePlayer(inputDirection);
    }

    void Update()
    {
        if (!isLocalPlayer || m_pHealth.m_isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(m_netID + " thats the id");
            m_pShoot.Shoot(m_dmgMod, m_netID);
        }


        Vector3 inputDirection = GetInput();

        if (inputDirection.sqrMagnitude > 0.25f)
        {
            m_pMotor.RotateChassis(inputDirection);
        }

        Vector3 turretDir = Utility.ScreenToWorldPoint(Input.mousePosition, m_pMotor.m_turret.position.y) - m_pMotor.m_turret.position;
        m_pMotor.RotateTurret(turretDir);


        if (Input.GetKeyDown(KeyCode.T))
            UpdateUI(true);


        if (Input.GetKeyUp(KeyCode.T))
            UpdateUI(false);


    }

    Vector3 GetInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        return new Vector3(h, 0, v);
    }

    private void Disable()
    {
        Debug.Log("Player Died");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        transform.position = GetRandomSpawnPosition();
        m_pMotor.m_rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(5f);
        m_pHealth.Reset();
        if (m_spawnFx != null)
        {
            GameObject spawnFx = Instantiate(m_spawnFx, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            Destroy(spawnFx, 3f);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (m_spawnPoints != null)
        {
            if (m_spawnPoints.Length > 0)
            {
                NetworkStartPosition startPos = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];
                return startPos.transform.position;
            }
        }
        return m_originalPosition;
    }

    void UpdateUI(bool _panelActive)
    {
        //return if somehow key got pressed/released twice or similar
        if (_panelActive == m_InfoPanel.activeInHierarchy)
            return;

        if (m_InfoPanel.activeInHierarchy == true)
        {
            m_InfoPanel.SetActive(false);
            return;
        }
        else
            m_InfoPanel.SetActive(true);
        float dmgtaken = GetComponent<PlayerHealth>().m_damageTaken;
        float hphealed = GetComponent<PlayerHealth>().m_healthHealed;
        // todo: add list for hitrate, made out # of bullets fired + times of scores gotten
        string texxt = "HitRate: " + "HPHealed: " + hphealed + "DMGTaken: " + dmgtaken;
        //update UI, display ping and own stats
        m_InfoPanel.GetComponentsInChildren<Text>().Where(o => o.tag == "InfoText").FirstOrDefault().text
            = texxt;
    }
}
