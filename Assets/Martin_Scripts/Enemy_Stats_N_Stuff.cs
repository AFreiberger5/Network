using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Stats_N_Stuff : NetworkBehaviour
{
    public float mpu_CurrentHP = 5;
    private float mpi_MaxHP = 5;
    // Angriffspunkte des Gegners
    private float mpi_Attack = 10;
    // Verteidigungspunkte des Gegners
    private float mpi_Defense = 15;
    private Enemy_Navigaton mpi_Enav;
    private float mpi_AttackCoolDown = 0;

    private PlayerHealth mpi_PH;

    public List<GameObject> mpu_ObjLoot;

    public GameObject mpu_ObjCenter;
    public GameObject mpu_ObjFistAnkle;
    public GameObject mpu_ObjFistAnkleLeft;
    public GameObject mpu_ObjFistAnkleRight;
    public GameObject mpu_ObjFistLeft;
    public GameObject mpu_ObjFistRight;

    private bool mpi_ActiveAttacking = false;


    public float mpuP_HP
    {
        get
        {
            return mpu_CurrentHP;
        }
        set
        {
            mpu_CurrentHP = Mathf.Clamp(value, 0, mpi_MaxHP);
        }
    }

    // Use this for initialization
    void Awake()
    {
        // Currenthp auf MaxHP festlegen
        mpu_CurrentHP = mpi_MaxHP;

        // Enemy_Navigation script festlegen
        mpi_Enav = GetComponent<Enemy_Navigaton>();

    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        // zählt den CoolDown langsam nach oben.
        mpi_AttackCoolDown += 0.1f * Time.deltaTime;

        // Timer auto Reset
        if (mpi_AttackCoolDown >= 1)
        {
            mpi_AttackCoolDown = 0;
        }

        // Auf leben achten. Falls Leben kleiner gleich 0, Gegner tot.
        if (mpuP_HP <= 0)
        {
            // Gegner ZERSTÖREN!!!

            DropThatShit();
            Destroy(gameObject);
            OnNetworkDestroy();
        }

        // Falls Spieler bzw. Target in HitRange und das Ziel den Tag Player hat
        if (mpi_Enav.mpu_Target != null && mpi_Enav.mpu_Target.gameObject.tag == "Player")
        {
            if (GetDistanceTo(mpi_Enav.mpu_Target, gameObject.transform) <= 2f && mpi_AttackCoolDown >= 0.2f)
            {
                mpi_ActiveAttacking = !mpi_ActiveAttacking;
                Attack();
                mpi_AttackCoolDown = 0;
            }
        }
    }

    public void Attack()
    {
        // Schlaaaaag!

        // --- Optik ---
        if (mpi_ActiveAttacking)
        {
            mpu_ObjFistAnkleLeft.transform.Rotate(Vector3.up, 90);
            mpi_ActiveAttacking = false;
        }

        // --- Funktion ---

        // Holt sich den Script vom entsprechenden target!
        mpi_PH = mpi_Enav.mpu_Target.GetComponentInParent<PlayerHealth>();

        // DAMAAAAAAGE!
        mpi_PH.Damage(-10);
        
        
    }

    public void CalculateDamage(float _DAMAGE)
    {
        mpuP_HP += _DAMAGE;
    }

    public float GetDistanceTo(Transform _Target, Transform _Start)
    {
        // rechnet die tatsächliche entfernung zwischen ziel und Start aus.
        return Vector3.Distance(_Target.position, _Start.position);
    }


    public override bool OnSerialize(NetworkWriter _writer, bool _initial)
    {
        _writer.Write(mpu_CurrentHP);

        return true;
    }

    public override void OnDeserialize(NetworkReader _reader, bool _initialState)
    {
        mpu_CurrentHP = _reader.ReadInt32();
    }

    public void DropThatShit()
    {
        // Nach erfolgreichem Ausschalten eines Gegners muss ein zufälliger Loot gedroppt werden
        Vector3 DropPos = new Vector3(transform.position.x, 1, transform.position.z);

        // Zufällige Zahl zwischen 0 und der maximalen anzahl an Dropbaren Objekte. In unserem Fall 2. 
        // Sprich 1, wegen Index und so
        int Index = Random.Range(0,1);


        GameObject Drop = Instantiate(mpu_ObjLoot[0], DropPos, Quaternion.identity);
        NetworkServer.Spawn(Drop);

    }
}
