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

    public GameObject mpu_ObjCenter;
    public GameObject mpu_ObjFistAnkle;
    public GameObject mpu_ObjFistAnkleLeft;
    public GameObject mpu_ObjFistAnkleRight;
    public GameObject mpu_ObjFistLeft;
    public GameObject mpu_ObjFistRight;


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
        // Auf leben achten. Falls Leben kleiner gleich 0, Gegner tot.
        if (mpuP_HP <= 0)
        {
            // Gegner ZERSTÖREN!!!
            Destroy(gameObject);
            OnNetworkDestroy();
        }
        
        // Falls Spieler bzw. Target in HitRange
        if (mpi_Enav.mpu_Target != null)
        {
            if (GetDistanceTo(mpi_Enav.mpu_Target, gameObject.transform) <= 1.5f)
            {
                Attack();
            }
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider _col)
    {
        if (_col.tag == "Teaball")
        {
            // Leben um 1 verringern bei Treffer mit Teekanne!
            mpuP_HP -= 1;
        }
    }

    public void Attack()
    {
        // Schlaaaaag!
        mpu_ObjFistAnkleLeft.transform.Rotate(Vector3.up, 90);
        // Bewege den arm zurück
        //mpu_ObjFistAnkleLeft.transform.Rotate(Vector3.up, 90 * Time.deltaTime);

        // entsprechenden Arm bewegen (rotieren)
        // Wenn dann der Colider der faust einen Spieler trifft
        // Calculate Dmg
        // Füge spieler XY den errechneten Schaden zu.
    }

    public float CalculateDamage()
    {
        return 9001f;
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
}
