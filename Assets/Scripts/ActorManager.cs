using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorManager : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject dialogue;

    private GameObject player;

    private Vector3 dialogueOffset;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogue = gameObject.FindChildWithName("Dialogue");
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueOffset = dialogue.transform.position;
    }

    public void MoveToGoal(GameObject goal) {
        agent.SetDestination(goal.transform.position);
    }



    // Update is called once per frame
    void Update()
    {
    }
}
