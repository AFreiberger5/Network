using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OrbPickups : NetworkBehaviour {

    void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.CompareTag("Pick Up"))
        {
            _col.gameObject.SetActive(false);
            // + gib dem Spieler + x-HP
        }
    }
}
