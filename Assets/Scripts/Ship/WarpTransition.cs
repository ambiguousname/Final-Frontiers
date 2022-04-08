using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpTransition : MonoBehaviour
{
    GameObject shipActors;
    GameObject player;
    GameObject ship;
    WarpAnimator warpAnimator;
    Vector3 playerOffset;
    Vector3 playerLook;
    Dictionary<string, Vector3> actorOffsets;
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ship = GameObject.Find("ShipPrefab");
        shipActors = GameObject.Find("ShipActors");
    }

    public void Warp(string levelName) {
        SetPositions();
        SceneManager.LoadSceneAsync("Warp", LoadSceneMode.Single);
        SceneManager.sceneLoaded += UpdatePositions;
    }

    private void SetPositions() {
        playerOffset = player.transform.position - ship.transform.position;
        playerLook = player.transform.eulerAngles;
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
        warpAnimator = GameObject.FindObjectOfType<WarpAnimator>();
        warpAnimator.SetReveal(500);
        warpAnimator.TransitionReveal(-50);
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
