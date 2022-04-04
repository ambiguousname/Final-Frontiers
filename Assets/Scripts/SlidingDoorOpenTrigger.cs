using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorOpenTrigger : DoorTrigger
{
    public GameObject connectedDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (!locked && tagsAllowed.Contains(other.tag)) {
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
