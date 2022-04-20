using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class OrderParser : MonoBehaviour
{
    // Different subjects:
    // Celestial body (plot course, change orbit, fire at?)
    // Star system (plot course)
    // Ship (plot course (towards, or collision), fire at, scan, hail, monitor)
    // Anomaly (plot course (towards, or collision), fire at, scan, hail)
    public List<OrderTarget> orderTargets;

    private GameObject _camera;
    private DialogueRunner _runner;
    private GameObject _selectText;

    private ActorManager _selected;

    bool ordersEnabled = false;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _runner = GameObject.FindObjectOfType<DialogueRunner>();
        _selectText = GameObject.Find("OrderText");
        _selectText.SetActive(false);
        ordersEnabled = false;
    }

    public void EPressed()
    {
        if (_selected != null) {
            _selectText.SetActive(false);
            _runner.StartDialogue(_selected.orderDialogue);
        }
    }

    private void Update()
    {
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit);
        if (ordersEnabled && !_runner.IsDialogueRunning && hit.transform.TryGetComponent(out _selected))
        {
            _selectText.SetActive(true);
        }
        else if (_selectText.activeInHierarchy){
            _selectText.SetActive(false);
            _selected = null;
        }
    }

    [YarnCommand("readyOrders")]
    public void ReadyOrders()
    {
        ordersEnabled = true;
    }

    [YarnCommand("stopOrders")]
    public void StopOrders() {
        ordersEnabled = false;
    }

    [YarnCommand("listOrders")]
    public void ListOrders(string tag) {
        List<string> orderID = new List<string>();
        List<string> orderText = new List<string>();
        foreach (OrderTarget target in orderTargets) {
            if (target.allowedOrders.Contains(tag)) {
                orderText.Add(target.orderName);
                orderID.Add(target.nodeTarget);
            }
        }
        orderText.Add("Nevermind.");
        GetComponent<DialogueCreator>().SwitchSpeaker(_selected.gameObject);
        GetComponent<DialogueCreator>().RunOptions(orderText.ToArray(), (int selected) => {
            Debug.Log(orderText[selected]);
            if (orderText[selected] != "Nevermind.") {
                _runner.StartDialogue(orderID[selected] + tag);
            }
        });
    }
}