using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The tag of any object to enter to ")]
    public string tagToTrigger = "Player";

    public string dialogueToStart;

    private DialogueRunner runner;
    // Start is called before the first frame update
    void Start()
    {
        runner = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagToTrigger) {
            runner.StartDialogue(dialogueToStart);
        }
    }
}
