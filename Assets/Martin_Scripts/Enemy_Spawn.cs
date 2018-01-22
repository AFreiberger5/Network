using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Spawn : NetworkBehaviour
{
    public bool ARSCH = false;

    public GameObject mpu_ObjSmallEnemy;
    public Transform[] mpu_SpawnerObjects;
    public int mpu_PlayersOnline = 0;
    Vector3[] mpi_Spawns;

    public struct SpawnSorter
    {
        public int MaxSpawners;
        public bool ActivePlayer_1;
        public bool ActivePlayer_2;
        public bool ActivePlayer_3;
        public bool ActivePlayer_4;

        public SpawnSorter(int _MaxSpawners, bool _ActivePlayer_1, bool _ActivePlayer_2, bool _ActivePlayer_3, bool _ActivePlayer_4)
        {
            MaxSpawners = _MaxSpawners;
            ActivePlayer_1 = _ActivePlayer_1;
            ActivePlayer_2 = _ActivePlayer_2;
            ActivePlayer_3 = _ActivePlayer_3;
            ActivePlayer_4 = _ActivePlayer_4;
        }
    }


    // Use this for initialization
    void Awake()
    {
        mpu_PlayersOnline = 1;

        // Daten zur Verarbeitung des Spawns müssen vom Server übermittelt werden!
        SpawnSorter SSS = new SpawnSorter(1, true, false, false, false);

        mpi_Spawns = GetPointsToSpawn(SSS);

        // Variable muss von anderem skript später geändert werden!
        mpu_PlayersOnline = 1;
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ARSCH = true;
        }

        if (ARSCH)
        {
            StartCoroutine(EnemySpawnInSeconds(1));
            ARSCH = false;
        }
    }

    
    public Vector3[] GetPointsToSpawn(SpawnSorter _s)
    {
        Vector3[] SpawnItNow = new Vector3[_s.MaxSpawners];

        if (mpu_PlayersOnline == 1)
        {
            // nur 1 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
        }

        if (mpu_PlayersOnline == 2)
        {
            // nur 2 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
            SpawnItNow[1] = mpu_SpawnerObjects[2].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 2

        }

        if (mpu_PlayersOnline == 3)
        {
            // nur 3 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
            SpawnItNow[1] = mpu_SpawnerObjects[2].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 2
            SpawnItNow[2] = mpu_SpawnerObjects[0].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 3

        }

        if (mpu_PlayersOnline == 4)
        {
            // nur 4 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
            SpawnItNow[1] = mpu_SpawnerObjects[2].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 2
            SpawnItNow[2] = mpu_SpawnerObjects[0].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 3
            SpawnItNow[3] = mpu_SpawnerObjects[1].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 4

        }

        if (mpu_PlayersOnline < 1 || mpu_PlayersOnline > 4)
        {
            throw new System.Exception("Massiver Fehler am Stissl! Es muss mindestens ein Spieler Online sein. Maximal dürfen vier Spieler Online sein. Außerdem kann es sein, dass der Server bzw. die Clients nicht vermittelt bekommen, wie viele spieler Online sind!");
        }

        return SpawnItNow;
    }

    IEnumerator EnemySpawnInSeconds(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        // Wenn die Gegner spawnen können
        foreach (Vector3 V in mpi_Spawns)
        {
            GameObject GO = Instantiate(mpu_ObjSmallEnemy, V, Quaternion.identity);
            NetworkServer.Spawn(GO);
        }

    }
}
