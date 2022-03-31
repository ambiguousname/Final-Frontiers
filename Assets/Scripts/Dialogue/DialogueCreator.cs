using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.DialogueTrees;

public class DialogueCreator : MonoBehaviour
{
    public string DialogueToStart;

    private DialogueTreeController tree;

    private GameObject activeDialogue;
    private Text activeName;
    private Text activeText;
    private GameObject continueText;
    private SubtitlesRequestInfo activeInfo;
    private MultipleChoiceRequestInfo activeChoice;

    private GameObject player;

    private GameObject dialogueLocator;

    private Rect canvasRect;

    private IEnumerator activeTextPrint;

    bool dialogueFinished = true;

    public void StartNewDialogue(string treeName) {
        if (dialogueFinished) {
            GameObject dialogueObject = gameObject.FindChildWithName(treeName);
            if (dialogueObject != null)
            {
                tree = dialogueObject.GetComponent<DialogueTreeController>();
                dialogueFinished = false;
                tree.StartDialogue();
            } else
            {
                Debug.LogWarning("Could not find DialogueTree of name " + treeName);
            }
        }
    }

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
        if (DialogueToStart != "") {
            tree = gameObject.FindChildWithName(DialogueToStart).GetComponent<DialogueTreeController>();
            dialogueFinished = false;
            tree.StartDialogue();
        }
    }

    public DialogueTreeController GetTree() {
        return tree;
    }

    private bool DialogueIsVisible() {
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
            if (activeChoice != null && activeChoice.options.Count >= number)
            {
                activeChoice.SelectOption(number - 1);
                activeChoice = null;
            }
            else if (number == 1)
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

    private void UpdateDialogue(SubtitlesRequestInfo info)
    {
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
        }
        continueText.SetActive(false);
        activeName.text = info.actor.name;
        activeText.text = "";
        activeText.color = info.actor.dialogueColor;
        activeTextPrint = AddText(info.statement.text, activeText);
        StartCoroutine(activeTextPrint);
    }

    private void DialogueFinish(DialogueTree dlg) {
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
