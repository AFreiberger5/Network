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

    private float mpi_CoolDown = 0;

    public List<GameObject> mpu_Loot;

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
        mpi_CoolDown += 0.1f * Time.deltaTime;

        // Timer reset
        if (mpi_CoolDown >= 2f)
        {
            mpi_CoolDown = 0;
        }

        // Auf leben achten. Falls Leben kleiner gleich 0, Gegner tot.
        if (mpuP_HP <= 0)
        {
            // Gegner muss Loot fallen lassen!
            int BitteBitteGanzVielLootJa = Random.Range(1,10);

            // Wenn der Zufall es zulässt...
            if (BitteBitteGanzVielLootJa <= 6)
            {
                GameObject GO;

                Vector3 LootPos = new Vector3(transform.position.x, 1, transform.position.z);

                // Noch mehr Aufteilen
                if (BitteBitteGanzVielLootJa <= 8)
                {
                    GO = Instantiate(mpu_Loot[0], LootPos, Quaternion.identity);
                }
                else
                {
                    GO = Instantiate(mpu_Loot[1], LootPos, Quaternion.identity);
                }

                NetworkServer.Spawn(GO);
            }

            // Gegner ZERSTÖREN!!!
            Destroy(gameObject);
            OnNetworkDestroy();
        }
        
        // Falls Spieler bzw. Target in HitRange
        if (mpi_Enav.mpu_Target != null)
        {
            if (GetDistanceTo(mpi_Enav.mpu_Target, gameObject.transform) <= 2.0f && mpi_CoolDown >= 0.2)
            {
                Attack(-5);
                mpi_CoolDown = 0;
            }
        }
    }

    public void Attack(float _Damage)
    {
        PlayerHealth PH = mpi_Enav.mpu_Target.GetComponentInParent<PlayerHealth>();

        PH.m_currentHealth += _Damage;
    }

    public void CalculateDamage(float _Damage)
    {
        mpuP_HP += _Damage;
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

    private void DropThatShit()
    {
        // Zufällige Chance
    }
}
