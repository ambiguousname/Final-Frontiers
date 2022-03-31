using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The tag of any object to enter to ")]
    public string tagToTrigger = "Player";

    public string dialogueToStart;

    private DialogueCreator creator;
    // Start is called before the first frame update
    void Start()
    {
        creator = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueCreator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagToTrigger) {
            creator.StartNewDialogue(dialogueToStart);
        }
    }
}
