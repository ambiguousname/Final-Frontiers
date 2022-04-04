using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueCreator : DialogueViewBase
{
    public string DialogueToStart;

    private GameObject activeDialogue;
    private Text activeName;
    private Text activeText;
    private GameObject continueText;

    private string fullText;

    Action activeLine;
    Action<int> activeChoice;
    int numChoices;

    private GameObject player;

    private GameObject dialogueLocator;

    private Rect canvasRect;

    private IEnumerator activeTextPrint;

    bool dialogueFinished = true;
    bool canChoose = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueLocator = GameObject.Find("DialogueLocator");
        canvasRect = dialogueLocator.transform.parent.GetComponent<RectTransform>().rect;
    }

    private bool DialogueIsVisible() {
        // If the dialogue is ongoing, but invisible...
        if (!dialogueFinished && !activeDialogue.activeInHierarchy)
        {
            return true;
        }
        return Vector3.Dot(activeDialogue.transform.forward, player.transform.forward) > 0;
    }

    private void Update()
    {
        if (!dialogueFinished && !DialogueIsVisible())
        {
            if (!dialogueLocator.activeInHierarchy)
            {
                dialogueLocator.SetActive(true);
            }
            // Based on https://www.youtube.com/watch?v=BC3AKOQUx04

            Vector3 direction = player.transform.position - activeDialogue.transform.position;

            Quaternion rot = Quaternion.LookRotation(direction);
            rot.z = -rot.y;
            rot.x = 0;
            rot.y = 0;

            Vector3 northDir = new Vector3(0, 0, player.transform.eulerAngles.y);

            dialogueLocator.GetComponent<RectTransform>().localRotation = rot * Quaternion.Euler(northDir);

            dialogueLocator.GetComponent<RectTransform>().anchoredPosition = new Vector3(-dialogueLocator.transform.up.x * canvasRect.width / 2, -dialogueLocator.transform.up.y * canvasRect.height / 2, 0);
        }
        else if (DialogueIsVisible()) {
            dialogueLocator.SetActive(false);
        }
    }

    public void PressNumButton(int number) {
        if (DialogueIsVisible())
        {
            if (canChoose && numChoices >= number)
            {
                canChoose = false;
                activeDialogue.SetActive(false);
                activeChoice(number - 1);
            }
            else if (number == 1)
            {
                if (continueText.activeInHierarchy)
                {
                    activeDialogue.SetActive(false);
                    activeLine();
                }
                else
                {
                    StopCoroutine(activeTextPrint);
                    activeText.text = fullText;
                    continueText.SetActive(true);
                }
            }
        }
    }

    public override void DialogueStarted()
    {
        dialogueFinished = false;
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        activeChoice = onOptionSelected;
        canChoose = true;
        activeName.text = "You";
        activeText.text = "";
        continueText.SetActive(false);
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            activeText.text += (i + 1) + ". " + dialogueOptions[i].Line.TextWithoutCharacterName.Text + "\n";
        }
        numChoices = dialogueOptions.Length;
        activeDialogue.SetActive(true);
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        activeLine = onDialogueLineFinished;
        string name = dialogueLine.CharacterName;
        if (name != "You" && (activeDialogue == null || name != activeDialogue.name)) {
            activeDialogue = GameObject.Find(name).FindChildWithTag("ActorDialogueBox");
            activeName = activeDialogue.FindChildWithName("Name").GetComponent<Text>();
            activeText = activeDialogue.FindChildWithName("Text").GetComponent<Text>();
            continueText = activeDialogue.FindChildWithName("Continue");
        }
        activeDialogue.SetActive(true);
        continueText.SetActive(false);
        activeName.text = name;
        activeText.text = "";
        fullText = dialogueLine.TextWithoutCharacterName.Text;
        activeTextPrint = AddText(dialogueLine.TextWithoutCharacterName.Text, activeText);
        StartCoroutine(activeTextPrint);
    }

    public override void DialogueComplete()
    {
        activeDialogue.SetActive(false);
        dialogueFinished = true;
    }

    IEnumerator AddText(string t, Text ui) {
        for (int i = 0; i < t.Length; i++) {
            ui.text += t[i];

            if (t[i] == '.')
            {
                yield return new WaitForSeconds(0.2f);
            }
            else if (t[i] == ',') {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {

                yield return new WaitForSeconds(0.05f);
            }
        }

        yield return new WaitForSeconds(0.1f);
        continueText.SetActive(true);
    }


}
