using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;

public class StartDialogueOnAwake : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DialogueTreeController>().StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
