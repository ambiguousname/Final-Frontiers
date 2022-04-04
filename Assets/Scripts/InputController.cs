using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    Text title;
    InputField input;

    Action<string> activeCallback;

    private void Start()
    {
        title = gameObject.FindChildWithName("Text").GetComponent<Text>();
        input = gameObject.FindChildWithName("TextInput").GetComponent<InputField>();
    }

    public void CreateDialogue(string titleText, Action<string> callback) {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        transform.GetChild(0).gameObject.SetActive(true);
        title.text = titleText;
        activeCallback = callback;
    }

    // Start is called before the first frame update
    public void Submit(string text) {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (input.text.Length > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            activeCallback(text);
        }
    }
}
