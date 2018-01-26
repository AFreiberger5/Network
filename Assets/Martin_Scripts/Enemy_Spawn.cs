using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Spawn : NetworkBehaviour
{
    public bool ARSCH = false;

    // Gregors Liste PlayerData
    private List<GameObject> mpi_ObjPlayersOnline;

    // Liste mit SPawnPoints relativ zum aktuellen SpawnPoint
    private List<Vector3> mpi_SpawnPositionsAtPoint_1;
    private List<Vector3> mpi_SpawnPositionsAtPoint_2;
    private List<Vector3> mpi_SpawnPositionsAtPoint_3;
    private List<Vector3> mpi_SpawnPositionsAtPoint_4;

    // Die ultimative Liste, die alle Listen mit alternativen Spawnpunkten enthält.
    private List<List<Vector3>> mpi_UltimateSpawnerPositions;

    private SpawnSorter mpi_SpawnRegulation;
    public List<GameObject> mpu_ObjMonsters;
    public Transform[] mpu_SpawnerObjects;
    public int mpu_MaxEnemys = 3;
    Vector3[] mpi_Spawns;

    public int mpi_EnemyPerWave_Min = 3;
    public int mpi_EnemyPerWave_Max = 5;

    private float mpi_SpawnTimer = 0;
    private int mpi_PlayersCount;

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

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        mpi_PlayersCount = GameObject.FindGameObjectsWithTag("Player").Length / 4;

        // Daten zur Verarbeitung des Spawns müssen vom Server übermittelt werden!
        // SpawnSorter SSS = new SpawnSorter(2, true, true, true, true);
        mpi_SpawnRegulation = SpawnThemAll(mpi_PlayersCount);

        mpi_Spawns = GetPointsToSpawn(mpi_SpawnRegulation);

        // Alle Listen mit Positionen für den korrekten Spawn der Wellen, kann hier
        // korrekt und anständig angelegt werden. Siehe funktion. (per STRG + F12) XD
        AdjustSpawners();
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        // Anzahl der Spieler aktell halten.
        mpi_PlayersCount = GameObject.FindGameObjectsWithTag("Player").Length / 4;

        // SpawnSorter aktuell halten
        mpi_SpawnRegulation = SpawnThemAll(mpi_PlayersCount);

        // Liste mit Spawns aktualisieren
        mpi_Spawns = GetPointsToSpawn(mpi_SpawnRegulation);

        // Timer für den Spawnzyklus der Gegner wird gleichmäßig erhöht :D
        mpi_SpawnTimer += 0.1f * Time.deltaTime;

        // Wenn die Spanzeit erreicht wurde...

        //##########################################################################################
        // Hier ist der Timer. Wenn diese zeit erreicht wurde, dann kommen die Waves 
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
        
        // Für jeden Vector3 in der Liste der Spawner
            foreach (Vector3 V in mpi_Spawns)
            {
                // Erstelle eine Liste mit alternativen Vectoren
                List<Vector3> AltSpawns = SpawnProtection(V);

                // Für jeden Vector3 in der Liste, die gerade aus V erstellt wurde
                foreach (Vector3 AltVec in AltSpawns)
                {
                    // Zufallsvariable für spawn der Monster
                    int rndCount = Random.Range(0,11);

                    if (rndCount >= 8)
                    {
                        // Für jeden einzelnen existierenden Spawner werden nun Gegner gespawnt. Die Anzahl der zu spawnenden
                        // Gegner wird per Zufall festgelegt und nach und nach freigesetzt

                        int rnd = Random.Range(0, mpu_MaxEnemys);

                        GameObject GO = Instantiate(mpu_ObjMonsters[rnd], AltVec, Quaternion.identity);
                        NetworkServer.Spawn(GO);
                    }
                }
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

    private List<Vector3> SpawnProtection(Vector3 _CurrentSpawnPoint)
    {
        List<Vector3> SpawnPoints = new List<Vector3>();

        SpawnPoints.Add(new Vector3(_CurrentSpawnPoint.x + 1, _CurrentSpawnPoint.y, _CurrentSpawnPoint.z));
        SpawnPoints.Add(new Vector3(_CurrentSpawnPoint.x - 1, _CurrentSpawnPoint.y, _CurrentSpawnPoint.z));
        SpawnPoints.Add(new Vector3(_CurrentSpawnPoint.x, _CurrentSpawnPoint.y, _CurrentSpawnPoint.z + 1));
        SpawnPoints.Add(new Vector3(_CurrentSpawnPoint.x + 1, _CurrentSpawnPoint.y, _CurrentSpawnPoint.z - 1));
        SpawnPoints.Add(new Vector3(_CurrentSpawnPoint.x - 1, _CurrentSpawnPoint.y, _CurrentSpawnPoint.z + 1));

        return SpawnPoints;
    }

    private void AdjustSpawners()
    {
        if (mpi_Spawns[0] != null && mpi_SpawnPositionsAtPoint_1 != null)
        {
            mpi_SpawnPositionsAtPoint_1 = SpawnProtection(mpi_Spawns[0]);
        }

        if (mpi_Spawns[1] != null && mpi_SpawnPositionsAtPoint_2 != null)
        {
            mpi_SpawnPositionsAtPoint_2 = SpawnProtection(mpi_Spawns[1]);
        }

        if (mpi_Spawns[2] != null && mpi_SpawnPositionsAtPoint_3 != null)
        {
            mpi_SpawnPositionsAtPoint_3 = SpawnProtection(mpi_Spawns[2]);
        }

        if (mpi_Spawns[3] != null && mpi_SpawnPositionsAtPoint_4 != null)
        {
            mpi_SpawnPositionsAtPoint_4 = SpawnProtection(mpi_Spawns[3]);
        }
    }

}
