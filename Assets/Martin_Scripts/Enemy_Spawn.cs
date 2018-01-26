using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Spawn : NetworkBehaviour
{
    public bool ARSCH = false;

    // Gregors Liste PlayerData
    private List<GameObject> mpi_ObjPlayersOnline;

    public List<GameObject> mpu_ObjMonsters;
    public Transform[] mpu_SpawnerObjects;
    public int mpu_MaxEnemys = 3;
    Vector3[] mpi_Spawns;

    private float mpi_SpawnTimer = 0;

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
    void Start()
    {
        int ARGH = GameObject.FindGameObjectsWithTag("Player").Length / 4;

        // Daten zur Verarbeitung des Spawns müssen vom Server übermittelt werden!
        SpawnSorter SSS = new SpawnSorter(2, true, true, true, true);
        SpawnSorter SpawnEnemys = SpawnThemAll(ARGH);

        mpi_Spawns = GetPointsToSpawn(SpawnEnemys);
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        // Timer für den Spawnzyklus der Gegner wird gleichmäßig erhöht :D
        mpi_SpawnTimer += 0.1f * Time.deltaTime;

        // Wenn die Spanzeit erreicht wurde...
        if (mpi_SpawnTimer >= 0.5f)
        {
            ARSCH = true;
            mpi_SpawnTimer = 0;
        }

        // Spawn Gegner
        if (ARSCH)
        {
            StartCoroutine(EnemySpawnInSeconds(1));
            ARSCH = false;
        }
    }

    
    public Vector3[] GetPointsToSpawn(SpawnSorter _s)
    {
        Vector3[] SpawnItNow = new Vector3[_s.MaxSpawners];

        if (_s.MaxSpawners == 1)
        {
            // nur 1 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
        }

        if (_s.MaxSpawners == 2)
        {
            // nur 2 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
            SpawnItNow[1] = mpu_SpawnerObjects[2].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 2

        }

        if (_s.MaxSpawners == 3)
        {
            // nur 3 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
            SpawnItNow[1] = mpu_SpawnerObjects[2].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 2
            SpawnItNow[2] = mpu_SpawnerObjects[0].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 3

        }

        if (_s.MaxSpawners == 4)
        {
            // nur 4 Spieler
            SpawnItNow[0] = mpu_SpawnerObjects[3].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 1
            SpawnItNow[1] = mpu_SpawnerObjects[2].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 2
            SpawnItNow[2] = mpu_SpawnerObjects[0].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 3
            SpawnItNow[3] = mpu_SpawnerObjects[1].GetChild(0).transform.position;     // Spawnpos, Gegner gegenüber von Spieler 4

        }

        if (_s.MaxSpawners < 1 || _s.MaxSpawners > 4)
        {
            throw new System.Exception("Massiver Fehler am Stissl! Es muss mindestens ein Spieler Online sein. Maximal dürfen vier Spieler Online sein. Außerdem kann es sein, dass der Server bzw. die Clients nicht vermittelt bekommen, wie viele spieler Online sind!");
        }

        return SpawnItNow;
    }

    IEnumerator EnemySpawnInSeconds(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        //// Wenn die Gegner spawnen können
        foreach (Vector3 V in mpi_Spawns)
        {
            // Für jeden einzelnen existierenden Spawner werden nun Gegner gespawnt. Die Anzahl der zu spawnenden
            // Gegner wird per Zufall festgelegt und nach und nach freigesetzt

            int rnd = Random.Range(0, mpu_MaxEnemys);

            GameObject GO = Instantiate(mpu_ObjMonsters[rnd], V, Quaternion.identity);
            NetworkServer.Spawn(GO);
        }

        // Wenn spieler 1 aktiv ist und der Spawner gesetzt...
        if (mpi_Spawns[0] != null)
        {

        }

    }

    private SpawnSorter SpawnThemAll(int _MaxPlayers)
    {
        SpawnSorter Now = new SpawnSorter();

        // Die maximale anzahl an Spielern, die gerade InGame sind regelt, wie viele Spawnpunkte aktiv sind.
        //Now.MaxSpawners = mpi_ObjPlayersOnline.Count;
        Now.MaxSpawners = _MaxPlayers;

        // ist Spieler 1 Aktiv
        if (Now.MaxSpawners == 1)
        {
            Now.ActivePlayer_1 = true;
        }

        // ist Spieler 2 Aktiv
        if (Now.MaxSpawners == 2)
        {
            Now.ActivePlayer_1 = true;
            Now.ActivePlayer_2 = true;
        }

        // ist Spieler 3 Aktiv
        if (Now.MaxSpawners == 3)
        {
            Now.ActivePlayer_1 = true;
            Now.ActivePlayer_2 = true;
            Now.ActivePlayer_3 = true;
        }

        // ist Spieler 4 Aktiv
        if (Now.MaxSpawners == 4)
        {
            Now.ActivePlayer_1 = true;
            Now.ActivePlayer_2 = true;
            Now.ActivePlayer_3 = true;
            Now.ActivePlayer_4 = true;
        }

        // Kehre jetzt um!
        return Now;
    }
}
