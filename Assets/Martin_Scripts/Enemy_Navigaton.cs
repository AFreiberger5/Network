using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Enemy_Navigaton : NetworkBehaviour
{
    public int TargetLife = 1;

    private GameObject[] mpi_ObjPlayers;
    public GameObject mpu_ObjBaseTarget;
    public Transform mpu_Target;
    private NavMeshAgent mpi_Agent;

    // Use this for initialization
    void Awake()
    {
        // Setze den Arrayy mit den Spielern
        mpi_ObjPlayers = new GameObject[4];

        mpu_Target = mpu_ObjBaseTarget.transform;

        // Setze den agenten
        mpi_Agent = GetComponent<NavMeshAgent>();

        // falls der agent nicht gesetzt wurde, erstmal ALARM machen.
        if (mpi_Agent != null)
        {
            // Das ziel setzen
            SetTarget();
        }
        else
        {
            // ERROR
            throw new System.Exception("Der NavMeshAgent bei " + gameObject.name + " wurde nicht gesetzt bzw. nicht auf dem Objekt hinzugefügt!");
        }
    }

    [ServerCallback]
    private void Update()
    {
        // Das ziel aktuell halten
        if (!CheckforTargetsExistence(mpu_Target))
        {
            mpu_Target = null;
        }

        // Das Ziel immer aktuell festlegen

    }

    private void SetTarget()
    {
        // falls das target nicht null ist
        if (mpu_Target != null)
        {
            // Target nicht gesetzt. Base ansteuern!
            Vector3 TargetPos = GetTarget("Player");
            mpi_Agent.SetDestination(TargetPos);
        }
        else
        {
            Vector3 TargetPos = GetTarget("BaseTarget");
            mpi_Agent.SetDestination(TargetPos);
        }
    }

    private Vector3 GetTarget(string _Tag)
    {
        // Gibt die erstengefundene Position vom Objekt mit dem passenden Tag wieder.
        return GameObject.FindGameObjectsWithTag(_Tag)[0].transform.position;
    }

    private bool CheckforTargetsExistence(Transform _Target)
    {
        if (_Target.gameObject != null)
        {
            // Ziel existiert!
            return true;
        }
        else
        {
            // Ziel existiert nicht und muss genullt werden!
            return false;
        }
    }

}
