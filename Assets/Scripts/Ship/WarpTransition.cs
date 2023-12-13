using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class WarpTransition : MonoBehaviour
{
    GameObject shipActors;
    GameObject player;
    GameObject ship;
    GameObject cinemachine;
    WarpAnimator warpAnimator;
    Vector3 playerOffset;
    Vector3 playerLook;
    Vector3 cameraPitch;
    Dictionary<string, Vector3> actorOffsets;

    string warpDialogueToLoad;
    string levelToLoad;
    AsyncOperation levelLoading;

    private bool canExitWarp = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ship = GameObject.Find("ShipPrefab");
        shipActors = GameObject.Find("ShipActors");
        cinemachine = player.transform.GetChild(0).gameObject;
    }

    public void Warp(string levelName, string dialogueToStart) {
        SetPositions();
        levelToLoad = levelName;
        warpDialogueToLoad = dialogueToStart;
        // The Character Controller does not handle teleportation well. So we just disable it. We don't need to re-enable it, since the new
        // player will have their CharacterController enabled.
        player.GetComponent<CharacterController>().enabled = false;
        SceneManager.LoadScene("Warp", LoadSceneMode.Single);
        SceneManager.sceneLoaded += UpdatePositions;
        SceneManager.sceneLoaded += LoadYarnDialogue;

        canExitWarp = false;
        //StartCoroutine(WarpLoad());
    }

    private IEnumerator WarpLoad() {
        SceneManager.sceneLoaded -= UpdatePositions;
        SceneManager.sceneLoaded -= LoadYarnDialogue;

        levelLoading = SceneManager.LoadSceneAsync(levelToLoad);
        levelLoading.allowSceneActivation = false;
        yield return levelLoading;
    }

    public void ExitWarp() {
        SetPositions();
        SceneManager.LoadScene(levelToLoad);
    }

    private void SetPositions() {
        playerOffset = player.transform.position - ship.transform.position;
        playerLook = player.transform.eulerAngles;
        cameraPitch = cinemachine.transform.localEulerAngles;
        actorOffsets = new Dictionary<string, Vector3>(shipActors.transform.childCount);
        for (int i = 0; i < shipActors.transform.childCount; i++)
        {
            Transform child = shipActors.transform.GetChild(i);
            actorOffsets[child.name] = child.position - ship.transform.position;
        }
    }

    private void UpdatePositions(Scene scene, LoadSceneMode mode) {
        ship = GameObject.Find("ShipPrefab");
        shipActors = GameObject.Find("ShipActors");
        player = GameObject.FindGameObjectWithTag("Player");
        cinemachine = player.transform.GetChild(0).gameObject;
        cinemachine.transform.localRotation = Quaternion.Euler(cameraPitch);
        player.GetComponent<FirstPersonController>().SetPitch(cameraPitch.x);
        warpAnimator = GameObject.FindObjectOfType<WarpAnimator>();
        warpAnimator.SetReveal(0);
        warpAnimator.TransitionReveal(1);
        for (int i = 0; i < shipActors.transform.childCount; i++)
        {
            Transform child = shipActors.transform.GetChild(i);
            child.position = ship.transform.position + actorOffsets[child.name];
        }
        // Disable the new player's locomotion so they don't mess up the teleportation.
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = ship.transform.position + playerOffset;
        player.transform.rotation = Quaternion.Euler(playerLook);
        // Re-enable:
        player.GetComponent<CharacterController>().enabled = true;
    }

    private void LoadYarnDialogue(Scene scene, LoadSceneMode mode) {
        if (warpDialogueToLoad != null) {
            GameObject.FindObjectOfType<DialogueRunner>().startAutomatically = true;
            GameObject.FindObjectOfType<DialogueRunner>().startNode = warpDialogueToLoad;
        }
        GameObject.FindObjectOfType<WarpTransition>().levelToLoad = levelToLoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
