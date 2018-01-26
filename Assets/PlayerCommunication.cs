using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// probleme mit der spieler liste verzögerten die arbeit am chat
public class PlayerCommunication : NetworkBehaviour
{
    private InputField m_MessageInput;
    private GameObject m_MessageBox;
    private string m_Message;
    private SyncListPlayerData m_PlayerList;
    private PlayerData m_Data;

    private void Awake()
    {
        // sollte es dem spieler ermöglichen seine eigene hostid und namen zu finden
        m_PlayerList = FindObjectOfType<DataSyncFest>().m_PlayerData;
        foreach (PlayerData pd in m_PlayerList)
        {
            // ERROR: in der online scene kann ist die eigene hostid = null 
            // auch die supis wussten nicht warum
            // durch diesen fehler war es nicht mehr möglich eine spieler identifizierung stattfinden zu lassen

            //Debug.Log("server" + connectionToServer.hostId);// hier entsteht die magie

            //if (connectionToClient.hostId == pd.hostid)
            //{
            //    m_Data = pd;
            //}
        }

        m_MessageBox = GameObject.Find("MessageBox");

        m_MessageInput = GameObject.Find("MessageInput").GetComponent<InputField>();
        m_MessageInput.interactable = false;
        m_MessageInput.characterLimit = 40;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (m_MessageInput.interactable == false)// ermöglicht die eingabe
                {
                    m_MessageInput.interactable = true;
                    m_MessageInput.ActivateInputField();
                }
                else if (m_MessageInput.interactable == true)// schließt die eingabe und speichert die nachricht
                {
                    m_MessageInput.DeactivateInputField();

                    // --------------------------------------
                    m_Message = m_MessageInput.text;
                    //m_Message = m_Data.name + ": " + m_MessageInput.text;
                    CmdSendMessage(m_Message);
                    // --------------------------------------

                    m_MessageInput.text = "";
                    m_MessageInput.interactable = false;
                }
            }
        }
    }

    // schickt die nachricht an den server
    [Command]
    private void CmdSendMessage(string _message)
    {
        m_Message = _message;

        RpcBroadcastMessage(m_Message);
    }

    // empfängt die nachricht vom server und stellt sie dar
    [ClientRpc]
    private void RpcBroadcastMessage(string _message)
    {
        m_MessageBox.GetComponentInChildren<Text>().text = _message;
    }
}