using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpTransition : MonoBehaviour
{
    public GameObject shipActors;
    GameObject player;
    GameObject ship;
    WarpAnimator warpAnimator;
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ship = GameObject.Find("ShipPrefab");
        DontDestroyOnLoad(shipActors);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainCamera"));
        DontDestroyOnLoad(GameObject.Find("PlayerFollowCamera"));
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GameObject.Find("Canvas"));
    }

    public void Warp(string levelName) {
        Vector3 playerOffset = player.transform.position - ship.transform.position;
        Vector3[] actorOffsets = new Vector3[shipActors.transform.childCount];
        int i = 0;
        foreach (Transform t in shipActors.transform) {
            actorOffsets[i] = t.transform.position - ship.transform.position;
            i++;
        }
        SceneManager.LoadScene("Warp");
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
            ship = GameObject.Find("ShipPrefab");
            warpAnimator = GameObject.FindObjectOfType<WarpAnimator>();
            warpAnimator.SetReveal(50);
            warpAnimator.TransitionReveal(-50);
            i = 0;
            foreach (Transform t in shipActors.transform)
            {
                t.position = actorOffsets[i] + ship.transform.position;
                i++;
            }
            player.transform.position = ship.transform.position + playerOffset;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
