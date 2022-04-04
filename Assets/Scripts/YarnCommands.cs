using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    [YarnCommand("showInput")]
    public static void ShowInput(string title, string variable, string callbackNode) {
        GameObject.FindObjectOfType<InputController>().CreateDialogue(title, (string text) => {
            GameObject.FindObjectOfType<InMemoryVariableStorage>().SetValue(variable, text);
            GameObject.FindObjectOfType<DialogueRunner>().StartDialogue(callbackNode);
        });
    }

    [YarnCommand("lockDoor")]
    public static void LockDoor(GameObject door, bool shouldLock) {
        door.GetComponent<DoorTrigger>().locked = shouldLock;
    }

    [YarnFunction("contains")]
    public static bool Contains(string textToSearch, string textToMatch) {
        return textToSearch.Contains(textToMatch);
    }
}
