using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueInput : MonoBehaviour
{
    private DialogueCreator creator;
    private void Start()
    {
        creator = GameObject.Find("DialogueManager").GetComponent<DialogueCreator>();
    }
    void OnOne(InputValue value) {
        creator.PressNumButton(1);
    }

    void OnTwo() {
        creator.PressNumButton(2);
    }

    void OnThree() {
        creator.PressNumButton(3);
    }

    void OnFour() {
        creator.PressNumButton(4);
    }

    void OnFive() {
        creator.PressNumButton(5);
    }

    void OnSix() {
        creator.PressNumButton(6);
    }

    void OnSeven() {
        creator.PressNumButton(7);
    }

    void OnEight() {
        creator.PressNumButton(8);
    }
}
