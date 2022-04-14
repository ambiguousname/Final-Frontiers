using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpDownMenu : ACESMenu
{

    protected int _currentlySelected;

    protected struct MenuOption {
        public GameObject attachedObject;
        public TextMeshPro attachedText;
        public SpriteRenderer attachedRenderer;
        public Color prevObjectColor;
        public Color prevTextColor;
    }

    protected Button[] _buttons;
    protected List<MenuOption> selectedOptions;
    private Color _selectedColor;
    private Color _selectedTextColor;

    public void SetUp(Color selectedColor, Color selectedTextColor = default(Color)) {
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

    public override void SetOff()
    {
        for (int i = 0; i < selectedOptions.Count; i++) {
            selectedOptions[i].attachedObject.SetActive(false);
        }
    }

    protected void SetUpDownButtons()
    {
        _buttons[0].interactable = true;
        _buttons[3].interactable = true;
        _buttons[0].GetComponentInChildren<Text>().text = "Up";
        _buttons[3].GetComponentInChildren<Text>().text = "Down";
        if (_currentlySelected == 0)
        {
            _buttons[0].GetComponentInChildren<Text>().text = "";
            _buttons[0].GetComponent<Button>().interactable = false;
        }

        if (_currentlySelected >= selectedOptions.Count - 1)
        {
            _buttons[3].GetComponentInChildren<Text>().text = "";
            _buttons[3].GetComponent<Button>().interactable = false;
        }
    }

    public override IEnumerator Draw()
    {
        for (int i = 0; i < selectedOptions.Count; i++)
        {
            selectedOptions[i].attachedObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        SetUpDownButtons();
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

    protected void MoveUp() {
        if (_currentlySelected > 0) {
            if (selectedOptions[_currentlySelected].attachedText != null) {
                selectedOptions[_currentlySelected].attachedText.color = selectedOptions[_currentlySelected].prevTextColor;
            }
            selectedOptions[_currentlySelected].attachedRenderer.color = selectedOptions[_currentlySelected].prevObjectColor;
            StartCoroutine(MoveInDirection(-1));
        }

        if (_currentlySelected == 0)
        {
            _buttons[0].GetComponentInChildren<Text>().text = "";
            _buttons[0].GetComponent<Button>().interactable = false;
        }
        else if (_currentlySelected < selectedOptions.Count - 1)
        {
            _buttons[3].GetComponentInChildren<Text>().text = "Down";
            _buttons[3].GetComponent<Button>().interactable = true;
        }
    }

    protected void MoveDown() {
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
            _buttons[3].GetComponentInChildren<Text>().text = "";
            _buttons[3].GetComponent<Button>().interactable = false;
        }
        else if (_currentlySelected > 0)
        {
            _buttons[0].GetComponentInChildren<Text>().text = "Up";
            _buttons[0].GetComponent<Button>().interactable = true;
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
