using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpDownMenu : ACESMenu
{

    protected int _currentlySelected;

    private struct MenuOption {
        public GameObject attachedObject;
        public TextMeshPro attachedText;
        public SpriteRenderer attachedRenderer;
        public Color prevObjectColor;
        public Color prevTextColor;
    }

    private GameObject upButton;
    private GameObject downButton;
    private List<MenuOption> selectedOptions;
    private Color _selectedColor;
    private Color _selectedTextColor;

    public void SetUp(GameObject uButton, GameObject dButton, Color selectedColor, Color selectedTextColor = default(Color)) {
        upButton = uButton;
        downButton = dButton;
        _selectedColor = selectedColor;
        _selectedTextColor = selectedTextColor;
        selectedOptions = new List<MenuOption>();
    }

    protected void AddMenuOption(GameObject option) {
        MenuOption o = new MenuOption();
        o.attachedObject = option;
        o.attachedObject.TryGetComponent<TextMeshPro>(out o.attachedText);
        o.attachedRenderer = option.GetComponent<SpriteRenderer>();
        o.prevObjectColor = o.attachedRenderer.color;
        if (o.attachedText != null)
        {
            o.prevTextColor = o.attachedText.color;
        }

        if (selectedOptions.Count == _currentlySelected) {
            o.attachedRenderer.color = _selectedColor;
            if (o.attachedText != null) {
                o.attachedText.color = _selectedTextColor;
            }
        }

        selectedOptions.Add(o);
    }

    public override IEnumerator SetOff()
    {
        for (int i = 0; i < selectedOptions.Count; i++) {
            selectedOptions[i].attachedObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public override IEnumerator Draw()
    {
        for (int i = 0; i < selectedOptions.Count; i++)
        {
            selectedOptions[i].attachedObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public override void ButtonsCallback(int number)
    {
        if (number == 1)
        {
            MoveUp();
        }

        if (number == 4)
        {
            MoveDown();
        }
    }

    public void MoveUp() {
        if (_currentlySelected > 0) {
            if (selectedOptions[_currentlySelected].attachedText != null) {
                selectedOptions[_currentlySelected].attachedText.color = selectedOptions[_currentlySelected].prevTextColor;
            }
            selectedOptions[_currentlySelected].attachedRenderer.color = selectedOptions[_currentlySelected].prevObjectColor;
            StartCoroutine(MoveInDirection(-1));
        }

        if (_currentlySelected == 0)
        {
            upButton.transform.GetChild(0).gameObject.SetActive(false);
            upButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            downButton.transform.GetChild(0).gameObject.SetActive(true);
            downButton.GetComponent<Button>().interactable = true;
        }
    }

    public void MoveDown() {
        if (_currentlySelected < selectedOptions.Count - 1)
        {
            if (selectedOptions[_currentlySelected].attachedText != null)
            {
                selectedOptions[_currentlySelected].attachedText.color = selectedOptions[_currentlySelected].prevTextColor;
            }
            selectedOptions[_currentlySelected].attachedRenderer.color = selectedOptions[_currentlySelected].prevObjectColor;
            StartCoroutine(MoveInDirection(1));
        }

        if (_currentlySelected == selectedOptions.Count - 1)
        {
            downButton.transform.GetChild(0).gameObject.SetActive(false);
            downButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            upButton.transform.GetChild(0).gameObject.SetActive(true);
            upButton.GetComponent<Button>().interactable = true;
        }
    }

    IEnumerator MoveInDirection(int dir)
    {
        _currentlySelected += dir;
        yield return new WaitForSecondsRealtime(0.5f);
        selectedOptions[_currentlySelected].attachedRenderer.color = _selectedColor;
        if (selectedOptions[_currentlySelected].attachedText != null)
        {
            selectedOptions[_currentlySelected].attachedText.color = _selectedTextColor;
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        selectedOptions = new List<MenuOption>();
        _currentlySelected = 0;
    }
}
