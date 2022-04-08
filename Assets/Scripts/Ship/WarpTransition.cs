using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ship = GameObject.Find("ShipPrefab");
        shipActors = GameObject.Find("ShipActors");
        cinemachine = player.transform.GetChild(0).gameObject;
    }

    public void Warp(string levelName) {
        SetPositions();
        //Application.backgroundLoadingPriority = ThreadPriority.Low;
        SceneManager.LoadScene("Warp", LoadSceneMode.Single);
        SceneManager.sceneLoaded += UpdatePositions;
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
        player.transform.position = ship.transform.position + playerOffset;
        player.transform.rotation = Quaternion.Euler(playerLook);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
