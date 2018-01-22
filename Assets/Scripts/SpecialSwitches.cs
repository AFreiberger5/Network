using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpecialSwitches : NetworkBehaviour
{
    [SerializeField]
    private Collider SwitchActivated;
    // Use this for initialization
    void Start()
    {
        SwitchActivated = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {

    }

    void OnTriggerEnter(Collider _col)
    {

        if (_col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                SwitchActivated.enabled = !SwitchActivated.enabled;

            }

        }
    }
}
