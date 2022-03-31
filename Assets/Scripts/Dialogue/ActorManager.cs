using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorManager : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject dialogue;

    private GameObject player;

    private float dialogueRadius;

    private float dialogueY;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogue = gameObject.FindChildWithName("Dialogue");
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 offset = dialogue.transform.position - this.transform.position;

        dialogueRadius = Vector3.Magnitude(new Vector3(offset.x, 0, offset.z));
        dialogueY = dialogue.transform.localPosition.y;
    }

    public void Escort(GameObject escortTarget, float maximumDist) { 
        
    }



    // Update is called once per frame
    void Update()
    {
        if (dialogue.activeInHierarchy)
        {
            Vector3 direction = this.transform.position - player.transform.position;
            Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
            directionXZ.Normalize();

            dialogue.transform.localPosition = -directionXZ * dialogueRadius + new Vector3(0, dialogueY, 0);

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            dialogue.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        
    }
}
