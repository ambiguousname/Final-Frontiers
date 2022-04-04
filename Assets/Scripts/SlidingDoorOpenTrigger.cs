using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorOpenTrigger : MonoBehaviour
{
    public string tagAllowed = "Player";
    public GameObject connectedDoor;
    bool open = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagAllowed) {
            open = true;
            connectedDoor.GetComponent<Animation>().Play("DoorOpen");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (open) {
            open = false;
            connectedDoor.GetComponent<Animation>().Play("DoorClose");
        }
    }
}
