using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorManager : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToGoal(GameObject goal) {
        agent.Move(goal.transform.position);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
