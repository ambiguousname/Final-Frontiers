using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembraneDoorTrigger : DoorTrigger
{
    private BoxCollider coll;
    float targetAlpha = 1;
    Material currMaterial;

    private void Start()
    {
        coll = transform.GetChild(0).GetComponent<BoxCollider>();
        currMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!locked && tagsAllowed.Contains(other.tag)) {
            coll.enabled = false;
            targetAlpha = 0;
        }
    }

    private void OnTriggerExit() {
        coll.enabled = true;
        targetAlpha = 1;
    }

    private void Update()
    {
        currMaterial.SetFloat("_Alpha", Mathf.Lerp(currMaterial.GetFloat("_Alpha"), targetAlpha, Time.deltaTime * 5));
    }
}
