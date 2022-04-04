using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public List<string> tagsAllowed = new List<string>(new string[] { "Player", "Actor"});
    public bool locked = false;
    protected bool open = false;
}
