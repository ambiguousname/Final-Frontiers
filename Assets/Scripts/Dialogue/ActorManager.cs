using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class ActorManager : MonoBehaviour
{

    private NavMeshAgent agent;

    private GameObject activeShip;

    private GameObject meshShip;

    private GameObject duplicateAgent;

    private GameObject dialogue;

    private GameObject player;

    private float dialogueRadius;

    private float dialogueY;

    private GameObject escort;
    private float escortDist;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogue = gameObject.FindChildWithName("Dialogue");
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 offset = dialogue.transform.position - this.transform.position;

        dialogueRadius = Vector3.Magnitude(new Vector3(offset.x, 0, offset.z));
        dialogueY = dialogue.transform.position.y;
    }

    [YarnCommand("escort")]
    public void Escort(GameObject moveTarget, GameObject escortTarget, float maximumDist) {
        escort = escortTarget;
        escortDist = maximumDist;
        MoveTo(moveTarget);
    }

    private void EscortUpdate() {
        if (Vector3.Distance(this.transform.position, escort.transform.position) > escortDist)
        {
            agent.isStopped = true;
        }
        else if (agent.isStopped) {
            agent.isStopped = false;
        }
    }

    [YarnCommand("moveTo")]
    public void MoveTo(GameObject target) {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
    }

    [YarnCommand("startWalkOnShip")]
    private void StartWalkOnShip(GameObject currShip, GameObject navMeshShip) {
        activeShip = currShip;
        meshShip = navMeshShip;
        duplicateAgent = new GameObject();
        Vector3 offset = activeShip.transform.position - this.transform.position;
        duplicateAgent.transform.position = meshShip.transform.position + offset;
        agent = duplicateAgent.AddComponent<NavMeshAgent>();
    }

    private void WalkOnShipUpdate() {
        Vector3 offset = meshShip.transform.position - activeShip.transform.position;
        this.transform.position = duplicateAgent.transform.position + offset;
    }

    [YarnCommand("endWalkOnShip")]
    private void EndWalkOnShip() {
        activeShip = null;
        meshShip = null;
        Destroy(duplicateAgent);
        agent = GetComponent<NavMeshAgent>();
    }



    // Update is called once per frame
    void Update()
    {
        if (duplicateAgent != null) {
            WalkOnShipUpdate();
        }
        if (dialogue.activeInHierarchy)
        {
            Vector3 direction = this.transform.position - player.transform.position;
            Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
            directionXZ.Normalize();

            Vector3 newPos = this.transform.position - directionXZ * dialogueRadius;

            dialogue.transform.position = new Vector3(newPos.x, dialogueY, newPos.z);

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            dialogue.transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        if (escort != null) {
            EscortUpdate();
        }
    }
}
