using UnityEngine.Networking;

public class DataSyncFest : NetworkBehaviour
{
    // sync liste der player daten
    // das gameobject wandert von der offline- zur onlinescene
    public SyncListPlayerData m_PlayerData = new SyncListPlayerData();

    private void Awake()
    {
        // load immunity
        DontDestroyOnLoad(gameObject);
    }
}

public class SyncListPlayerData : SyncListStruct<PlayerData> { }

// struct zur speicherung von player relevanten daten
public struct PlayerData
{
    public string name;
    public int hostid;
}