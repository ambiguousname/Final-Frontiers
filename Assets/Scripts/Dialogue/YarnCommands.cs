using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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

    [YarnFunction("length")]
    public static int Length(string text) {
        return text.Length;
    }

    [YarnCommand("loadScene")]
    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    [YarnCommand("startAnim")]
    public static void StartAnim(GameObject gameObject, string animName) {
        gameObject.GetComponent<Animator>().Play(animName);
    }

    [YarnCommand("startBakedAnim")]
    public static void StartBakedAnim(GameObject gameObject, string animToPlay, string onComplete) {
        gameObject.GetComponent<BakedAnimPlayer>().StartAnim(animToPlay, () => {
            GameObject.FindObjectOfType<DialogueRunner>().StartDialogue(onComplete);
        });
    }

    [YarnCommand("shipWarp")]
    public static void ShipWarp(string sceneName, string dialogueToStart = null) {
        GameObject.FindObjectOfType<WarpTransition>().Warp(sceneName, dialogueToStart);
    }

    [YarnCommand("addDataBlock")]
    public static void AddDataBlock(string blockName) {
        GameObject.FindObjectOfType<ACESController>().dataBlockController.AddDataBlock(Resources.Load<TextAsset>("DataBlocks/" + blockName));
    }

    [YarnCommand("placeActor")]
    public static void PlaceActor(string actorName, GameObject whereToPlace) {
        var actor = Resources.Load<GameObject>("Actors/" + actorName);
        var shipActors = GameObject.Find("ShipActors");
        NavMeshHit hit;
        NavMesh.SamplePosition(whereToPlace.transform.position, out hit, 1.0f, 1);
        var actorObject = Instantiate(actor, hit.position, actor.transform.rotation, shipActors.transform);
        actorObject.name = actorName;
    }

    [YarnCommand("enableTrigger")]
    public static void EnableTrigger(GameObject trigger, string dialogue) {
        trigger.GetComponent<DialogueTrigger>().dialogueToStart = dialogue;
    }
}
