using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.DialogueTrees;

public class DialogueCreator : MonoBehaviour
{
    private GameObject activeDialogue;
    private Text activeName;
    private Text activeText;
    private GameObject continueText;
    private Renderer dialogueRenderer;
    private SubtitlesRequestInfo activeInfo;
    private MultipleChoiceRequestInfo activeChoice;

    private GameObject player;

    private GameObject dialogueLocator;

    private Rect canvasRect;

    private IEnumerator activeTextPrint;

    // Start is called before the first frame update
    void OnEnable()
    {
        DialogueTree.OnDialogueStarted += DialogueStart;
        DialogueTree.OnSubtitlesRequest += UpdateDialogue;
        DialogueTree.OnDialogueFinished += DialogueFinish;
        DialogueTree.OnMultipleChoiceRequest += MultipleChoice;
    }

    private void OnDisable()
    {
        DialogueTree.OnDialogueStarted -= DialogueStart;
        DialogueTree.OnSubtitlesRequest -= UpdateDialogue;
        DialogueTree.OnDialogueFinished -= DialogueFinish;
        DialogueTree.OnMultipleChoiceRequest -= MultipleChoice;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueLocator = GameObject.Find("DialogueLocator");
        canvasRect = dialogueLocator.transform.parent.GetComponent<RectTransform>().rect;
    }

    private void Update()
    {
        if (activeDialogue.activeInHierarchy && !dialogueRenderer.isVisible)
        {
            if (!dialogueLocator.activeInHierarchy)
            {
                dialogueLocator.SetActive(true);
            }
            // Based on https://www.youtube.com/watch?v=BC3AKOQUx04

            Vector3 direction = player.transform.position - dialogueRenderer.transform.position;

            Quaternion rot = Quaternion.LookRotation(direction);
            rot.z = -rot.y;
            rot.x = 0;
            rot.y = 0;

            Vector3 northDir = new Vector3(0, 0, player.transform.eulerAngles.y);

            dialogueLocator.GetComponent<RectTransform>().localRotation = rot * Quaternion.Euler(northDir);

            dialogueLocator.GetComponent<RectTransform>().anchoredPosition = new Vector3(-dialogueLocator.transform.up.x * canvasRect.width / 2, -dialogueLocator.transform.up.y * canvasRect.height / 2, 0);
        }
        else if (dialogueRenderer.isVisible && dialogueLocator.activeInHierarchy) {
            dialogueLocator.SetActive(false);
        }
    }

    public void PressNumButton(int number) {
        if (dialogueRenderer.isVisible)
        {
            if (activeChoice != null && activeChoice.options.Count >= number)
            {
                activeChoice.SelectOption(number - 1);
                activeChoice = null;
            }

            if (number == 1)
            {
                if (continueText.activeInHierarchy)
                {
                    activeInfo.Continue();
                }
                else
                {
                    StopCoroutine(activeTextPrint);
                    activeText.text = activeInfo.statement.text;
                    continueText.SetActive(true);
                }
            }
        }
    }

    private void DialogueStart(DialogueTree dlg) {

    }

    private void MultipleChoice(MultipleChoiceRequestInfo info) {
        activeChoice = info;
        activeName.text = "You";
        activeText.text = "";
        continueText.SetActive(false);
        foreach (KeyValuePair<IStatement, int> choice in info.options) {
            activeText.text += (choice.Value + 1) + ". " + choice.Key + "\n";
        }
    }

    private void UpdateDialogue(SubtitlesRequestInfo info) {
        activeInfo = info;
        if (info.actor.name != "You" && (activeDialogue == null || info.actor.name != activeDialogue.name)) {
            if (activeDialogue != null)
            {
                activeDialogue.SetActive(false);
            }
            activeDialogue = GameObject.Find(info.actor.name).FindChildWithTag("ActorDialogueBox");
            activeDialogue.SetActive(true);
            activeName = activeDialogue.FindChildWithName("Name").GetComponent<Text>();
            activeText = activeDialogue.FindChildWithName("Text").GetComponent<Text>();
            continueText = activeDialogue.FindChildWithName("Continue");
            dialogueRenderer = activeDialogue.FindChildWithName("VisibleBox").GetComponent<Renderer>();
        }
        continueText.SetActive(false);
        activeName.text = info.actor.name;
        activeText.text = "";
        activeTextPrint = AddText(info.statement.text, activeText);
        StartCoroutine(activeTextPrint);
    }

    private void DialogueFinish(DialogueTree dlg) {
        activeDialogue.SetActive(false);
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
