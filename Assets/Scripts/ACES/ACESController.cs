using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ACESController : UpDownMenu
{
    [HideInInspector]
    public DataBlockController dataBlockController;

    Dictionary<string, GameObject> displays;
    Dictionary<string, Color> originalColor;

    ACESMenu _activeMenu;

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

        dataBlockController = gameObject.FindChildWithName("DataMenu").GetComponent<DataBlockController>();
        dataBlockController.InitController(displays["button1"], displays["button4"], displays["button2"].GetComponent<Button>(), displays["button5"].GetComponent<Button>(), displays["button6"].GetComponent<Button>());
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
        AddMenuDisplays("mail", gameObject.FindChildWithName("Mail"));
        AddMenuDisplays("data", gameObject.FindChildWithName("Data"));
        AddMenuDisplays("settings", gameObject.FindChildWithName("Settings"));
        AddMenuDisplays("menu", gameObject.FindChildWithName("MainMenu"));
        displays.Add("maps", gameObject.FindChildWithName("Maps"));
        displays.Add("games", gameObject.FindChildWithName("Games"));

        originalColor.Add("button1", displays["button1"].GetComponent<Image>().color);
        originalColor.Add("button2", displays["button2"].GetComponent<Image>().color);
        originalColor.Add("button3", displays["button3"].GetComponent<Image>().color);
        originalColor.Add("button4", displays["button4"].GetComponent<Image>().color);
        originalColor.Add("button5", displays["button5"].GetComponent<Image>().color);
        originalColor.Add("button6", displays["button6"].GetComponent<Image>().color);
        originalColor.Add("mail", displays["mail"].GetComponent<SpriteRenderer>().color);
        originalColor.Add("data", displays["data"].GetComponent<SpriteRenderer>().color);
        originalColor.Add("settings", displays["settings"].GetComponent<SpriteRenderer>().color);
        originalColor.Add("menu", displays["menu"].GetComponent<SpriteRenderer>().color);

        _activeMenu = this;

        SetUp(displays["button1"], displays["button4"], Color.white);
        
        MoveUp();
        ResetDisplay();
    }

    public override IEnumerator Draw() {
        string[] order = { "title", "time", "date", "messageCount", "mail", "data", "settings", "menu", "maps", "games", "button1", "button4", "button2", "button5", "button3", "button6" };
        for (int i = 0; i < order.Length; i++)
        {
            displays[order[i]].SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void ResetDisplay() {
        foreach (KeyValuePair<string, GameObject> display in displays) {
            display.Value.SetActive(false);
        }
    }

    public void PushButton(int number) {
        if (number == 6 && _activeMenu != this)
        {
            _activeMenu.SetOff();
            activeDisplayChange = _activeMenu.Draw();
            StartCoroutine(activeDisplayChange);
        }
        else if (number == 2 && _activeMenu == this)
        {
            Select();
        }
        else {
            _activeMenu.ButtonsCallback(number);
        }
    }

    public void Select()
    {
        switch (_currentlySelected) {
            case 1:
                _activeMenu = GetComponentInChildren<DataBlockController>();
                break;
            default:
                Debug.LogError("Unrecognized menu option " + _currentlySelected);
                break;
        }
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
        displays["time"].GetComponent<TextMeshPro>().text = currTime.ToString("HH") + "." + percentDisplay;
    }
}
