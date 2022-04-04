using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembraneDoorTrigger : DoorTrigger
{
    private BoxCollider coll;

    private void Start()
    {
        coll = transform.GetChild(0).GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!locked && other.tag == tagAllowed) {
            coll.enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit() {
        coll.enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }
}
