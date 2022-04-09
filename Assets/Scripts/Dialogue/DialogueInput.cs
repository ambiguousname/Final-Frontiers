using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueInput : MonoBehaviour
{
    private DialogueCreator creator;
    private GameObject menu;
    private void Start()
    {
        var manager = GameObject.Find("DialogueManager");
        if (manager != null)
        {
            GameObject.Find("DialogueManager").TryGetComponent<DialogueCreator>(out creator);
        }
        menu = GameObject.Find("Menu");
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

    void OnMenu() {
        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}
