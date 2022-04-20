using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class ActorManager : MonoBehaviour
{
    public bool canGiveOrders;
    public string orderDialogue;

    private NavMeshAgent agent;

    private GameObject dialogue;

    private GameObject player;

    private float dialogueRadius;

    private float dialogueY;

    private GameObject escort;
    private float escortDist;

    void OnEnable()
    {
        TryGetComponent<NavMeshAgent>(out agent);
    }

    private void Start()
    {
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
        this.agent.SetDestination(target.transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        if (dialogue.activeInHierarchy)
        {
            Vector3 direction = this.transform.position - player.transform.position;
            Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
            directionXZ.Normalize();

            Vector3 newPos = this.transform.position - directionXZ * dialogueRadius;

            dialogue.transform.position = new Vector3(newPos.x, dialogueY, newPos.z);

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            dialogue.transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 sizeDir = player.transform.position - this.transform.position;
            Vector3 sizeXZ = new Vector3(sizeDir.x, 0, sizeDir.z);
            Vector3 sizeVector = (sizeXZ)/4;
            float size = sizeVector.magnitude;
            if (size > 3) {
                size = 3;
            }
            dialogue.transform.localScale = new Vector3(size, size, 1);
        }

        if (escort != null) {
            EscortUpdate();
        }
    }
}
