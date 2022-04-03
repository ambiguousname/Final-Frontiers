using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    public bool canRepeat = false;

    [Tooltip("The tag of any object to enter to ")]
    public string tagToTrigger = "Player";

    public string dialogueToStart;

    bool hasPlayed = false;

    private DialogueRunner runner;
    // Start is called before the first frame update
    void Start()
    {
        runner = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueRunner>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasPlayed && other.tag == tagToTrigger && runner.Dialogue.IsActive == false) {
            runner.StartDialogue(dialogueToStart);
            if (!canRepeat) {
                hasPlayed = true;
            }
        }
    }
}
