using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ACESController : UpDownMenu
{
    [HideInInspector]
    public DataBlockController dataBlockController;
    [HideInInspector]
    public LogController logController;

    Dictionary<string, GameObject> displays;
    Dictionary<string, Color> originalColor;

    ACESMenu _activeMenu;
    Button[] _buttons;

    private IEnumerator activeDisplayChange;

    private void Awake()
    {
        displays = new Dictionary<string, GameObject>();
        originalColor = new Dictionary<string, Color>();
        displays.Add("button1", gameObject.FindChildWithName("Button1"));
        displays.Add("button2", gameObject.FindChildWithName("Button2"));
        displays.Add("button3", gameObject.FindChildWithName("Button3"));
        displays.Add("button4", gameObject.FindChildWithName("Button4"));
        displays.Add("button5", gameObject.FindChildWithName("Button5"));
        displays.Add("button6", gameObject.FindChildWithName("Button6"));

        _buttons = new Button[] { displays["button1"].GetComponent<Button>(), displays["button2"].GetComponent<Button>(), displays["button3"].GetComponent<Button>(),
        displays["button4"].GetComponent<Button>(), displays["button5"].GetComponent<Button>(), displays["button6"].GetComponent<Button>()};

        SetUp(_buttons, Color.white);

        dataBlockController = gameObject.FindChildWithName("DataMenu").GetComponent<DataBlockController>();
        dataBlockController.InitController(_buttons);
        logController = gameObject.FindChildWithName("LogMenu").GetComponent<LogController>();
    }

    private void AddMenuDisplays(string title, GameObject g) {
        displays.Add(title, g);
        this.AddMenuOption(g);
    }

    private void Start()
    {
        displays.Add("title", gameObject.FindChildWithName("ACESTitleBig"));
        displays.Add("time", gameObject.FindChildWithName("Time"));
        displays.Add("date", gameObject.FindChildWithName("Date"));
        displays.Add("messageCount", gameObject.FindChildWithName("MessageCount"));
        AddMenuDisplays("log", gameObject.FindChildWithName("Log"));
        AddMenuDisplays("data", gameObject.FindChildWithName("Data"));
        AddMenuDisplays("settings", gameObject.FindChildWithName("Settings"));
        AddMenuDisplays("menu", gameObject.FindChildWithName("MainMenu"));
        displays.Add("maps", gameObject.FindChildWithName("Maps"));
        displays.Add("games", gameObject.FindChildWithName("Games"));

        originalColor.Add("button1", _buttons[0].GetComponent<Image>().color);
        originalColor.Add("button2", _buttons[1].GetComponent<Image>().color);
        originalColor.Add("button3", _buttons[2].GetComponent<Image>().color);
        originalColor.Add("button4", _buttons[3].GetComponent<Image>().color);
        originalColor.Add("button5", _buttons[4].GetComponent<Image>().color);
        originalColor.Add("button6", _buttons[5].GetComponent<Image>().color);
        originalColor.Add("log", displays["log"].GetComponent<SpriteRenderer>().color);
        originalColor.Add("data", displays["data"].GetComponent<SpriteRenderer>().color);
        originalColor.Add("settings", displays["settings"].GetComponent<SpriteRenderer>().color);
        originalColor.Add("menu", displays["menu"].GetComponent<SpriteRenderer>().color);

        _activeMenu = this;
        SetUpDownButtons();
        ResetDisplay();
    }

    public override IEnumerator Draw() {
        string[] order = { "title", "time", "date", "messageCount", "log", "data", "settings", "menu", "maps", "games", "button1", "button4", "button2", "button5", "button3", "button6" };
        for (int i = 0; i < order.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            displays[order[i]].SetActive(true);
        }

        if (_currentlySelected > 0)
        {
            _buttons[0].interactable = true;
            _buttons[0].GetComponentInChildren<Text>().text = "Up";
        }

        if (_currentlySelected < selectedOptions.Count - 1)
        {
            _buttons[3].GetComponentInChildren<Text>().text = "Down";
            _buttons[3].interactable = true;
        }

        _buttons[1].interactable = true;
        _buttons[5].interactable = true;

        _buttons[1].GetComponentInChildren<Text>().text = "Select";
        _buttons[5].GetComponentInChildren<Text>().text = "Exit";
    }

    public void ResetDisplay() {
        foreach (KeyValuePair<string, GameObject> display in displays) {
            display.Value.SetActive(false);
        }
    }

    public override void ButtonsCallback(int number)
    {
        base.ButtonsCallback(number);
        if (number == 6) {
            ToggleShowMenu();
        }
        if (number == 2) {
            StartCoroutine(Select());
        }
    }

    public override void SetOff()
    {
        base.SetOff();
        displays["title"].SetActive(false);
        displays["time"].SetActive(false);
        displays["date"].SetActive(false);
        displays["messageCount"].SetActive(false);
        displays["games"].SetActive(false);
        displays["maps"].SetActive(false);

        for (int i = 0; i < _buttons.Length; i++) {
            _buttons[i].gameObject.GetComponentInChildren<Text>().text = "";
            _buttons[i].interactable = false;
        }
    }

    public void PushButton(int number) {
        if (number == 6 && _activeMenu != this)
        {
            _activeMenu.SetOff();
            _activeMenu = this;
            _buttons[5].GetComponentInChildren<Text>().text = "";
            _buttons[5].interactable = false;
            StopCoroutine(activeDisplayChange);
            activeDisplayChange = _activeMenu.Draw();
            StartCoroutine(activeDisplayChange);
        }
        else {
            _activeMenu.ButtonsCallback(number);
        }
    }

    public IEnumerator Select()
    {
        _activeMenu.SetOff();
        switch (_currentlySelected) {
            case 0:
                _activeMenu = logController;
                break;
            case 1:
                _activeMenu = dataBlockController;
                break;
            default:
                Debug.LogError("Unrecognized menu option " + _currentlySelected);
                break;
        }
        StopCoroutine(activeDisplayChange);
        yield return new WaitForSecondsRealtime(0.5f);
        _buttons[5].GetComponentInChildren<Text>().text = "Back";
        _buttons[5].interactable = true;
        activeDisplayChange = _activeMenu.Draw();
        StartCoroutine(activeDisplayChange);
    }

    public void ToggleShowMenu() {
        var active = this.transform.GetChild(0).gameObject.activeInHierarchy;
        var g = this.transform.GetChild(0).gameObject;
        if (activeDisplayChange != null)
        {
            StopCoroutine(activeDisplayChange);
        }
        if (active)
        {
            _activeMenu.SetOff();
            _activeMenu = this;
            activeDisplayChange = HideMenu(g);
            StartCoroutine(activeDisplayChange);
        }
        else
        {
            g.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            activeDisplayChange = Draw();
            StartCoroutine(activeDisplayChange);
        }
    }

    private IEnumerator HideMenu(GameObject g) {
        ResetDisplay();
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        g.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var currTime = System.DateTime.Now;
        var percentage = Mathf.Floor(100 * (currTime.Minute * 60 + currTime.Second)/3600);
        string percentDisplay = percentage.ToString();

        if (percentage < 10)
        {
            percentDisplay = "0" + percentDisplay;
        }

        displays["time"].GetComponent<TextMeshPro>().text = currTime.ToString("HH") + "." + percentDisplay;
    }
}
