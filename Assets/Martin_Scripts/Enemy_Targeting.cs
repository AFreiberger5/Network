using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Targeting : NetworkBehaviour
{
    public List<TargetInfoData> mpu_Players;
    public float RangeDistance;

    public struct TargetInfoData
    {
        public GameObject PlayerObject;
        public Transform PlayerTransform;
        public float PlayerDistance;

        public TargetInfoData(GameObject _PlayerObject, float _PlayerDistance)
        {
            PlayerObject = _PlayerObject;
            PlayerTransform = _PlayerObject.transform;
            PlayerDistance = _PlayerDistance;
        }
    }

    private void Start()
    {
        // Legt die Liste für die Spieler an
        mpu_Players = new List<TargetInfoData>();

        // Holt sich den Radius vom entsprechenden Colider
        RangeDistance = GetComponent<SphereCollider>().radius;
        // Packt noch 1 drauf, damit wir unseren Spieler auch einbezogen haben.
        RangeDistance += 1;
        // Quadrieren um Kosten zu sparen
        RangeDistance *= RangeDistance;
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            // Erstellt eine neue TargetInfoData mit den Infos der Kollidierenden Objekte. Sprich GameObject und der passenden Distance.
            TargetInfoData tmp = new TargetInfoData(_col.gameObject, GetDistance(_col.gameObject.transform.position, gameObject.transform.position));

            // Fügt die neue TargetInfoData der bestehenden Liste hinzu.
            mpu_Players.Add(tmp);
        }
    }

    [ServerCallback]
    private void Update()
    {
        // Entfernt alle Spieler aus der Liste, die zu weit entfernt sind :D

        //mpu_Players.RemoveAll(o => GetDistance(o.PlayerObject.transform.position, gameObject.transform.position) >= RangeDistance);

        foreach (TargetInfoData TID in mpu_Players)
        {
            if (GetDistance(TID.PlayerObject.transform.position, gameObject.transform.position) >= RangeDistance)
            {
                if (TID.PlayerObject != null)
                {
                    mpu_Players.Remove(TID);
                }

            }
        }
    }

    // Errechnet die Entfernung von zwei Objekten.
    public float GetDistance(Vector3 _Target, Vector3 _Start)
    {
        return (_Target - _Start).sqrMagnitude;
    }
}
