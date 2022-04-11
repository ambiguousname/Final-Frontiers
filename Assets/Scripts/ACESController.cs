using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ACESController : MonoBehaviour
{
    Dictionary<string, GameObject> displays;
    Dictionary<string, Color> originalColor;
    string[] menu = { "mail", "data", "settings", "menu" };
    int currMenuOption;

    private IEnumerator activeDisplayChange;

    private void Start()
    {
        displays = new Dictionary<string, GameObject>();
        originalColor = new Dictionary<string, Color>();
        displays.Add("title", gameObject.FindChildWithName("ACESTitleBig"));
        displays.Add("time", gameObject.FindChildWithName("Time"));
        displays.Add("date", gameObject.FindChildWithName("Date"));
        displays.Add("messageCount", gameObject.FindChildWithName("MessageCount"));
        displays.Add("mail", gameObject.FindChildWithName("Mail"));
        displays.Add("data", gameObject.FindChildWithName("Data"));
        displays.Add("settings", gameObject.FindChildWithName("Settings"));
        displays.Add("menu", gameObject.FindChildWithName("MainMenu"));
        displays.Add("maps", gameObject.FindChildWithName("Maps"));
        displays.Add("games", gameObject.FindChildWithName("Games"));
        displays.Add("button1", gameObject.FindChildWithName("Button1"));
        displays.Add("button2", gameObject.FindChildWithName("Button2"));
        displays.Add("button3", gameObject.FindChildWithName("Button3"));
        displays.Add("button4", gameObject.FindChildWithName("Button4"));
        displays.Add("button5", gameObject.FindChildWithName("Button5"));
        displays.Add("button6", gameObject.FindChildWithName("Button6"));

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

        currMenuOption = 0;
        displays[menu[currMenuOption]].GetComponent<SpriteRenderer>().color = Color.white;
        MoveUp();
        SetOff();
    }

    IEnumerator DrawMainMenu() {
        string[] order = { "title", "time", "date", "messageCount", "mail", "data", "settings", "menu", "maps", "games", "button1", "button4", "button2", "button5", "button3", "button6" };
        for (int i = 0; i < order.Length; i++) {
            displays[order[i]].SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void SetOff() {
        foreach (KeyValuePair<string, GameObject> display in displays) {
            display.Value.SetActive(false);
        }
    }

    public void MoveUp() {
        if (currMenuOption > 0) {
            displays[menu[currMenuOption]].GetComponent<SpriteRenderer>().color = originalColor[menu[currMenuOption]];
            StartCoroutine(MoveInDirection(-1));
        }

        if (currMenuOption == 0)
        {
            displays["button1"].transform.GetChild(0).gameObject.SetActive(false);
            displays["button1"].GetComponent<Button>().interactable = false;
        }
        else
        {
            displays["button4"].transform.GetChild(0).gameObject.SetActive(true);
            displays["button4"].GetComponent<Button>().interactable = true;
        }
    }

    public void MoveDown() {
        if (currMenuOption < menu.Length - 1) {
            displays[menu[currMenuOption]].GetComponent<SpriteRenderer>().color = originalColor[menu[currMenuOption]];
            StartCoroutine(MoveInDirection(1));
        }

        if (currMenuOption == menu.Length - 1)
        {
            displays["button4"].transform.GetChild(0).gameObject.SetActive(false);
            displays["button4"].GetComponent<Button>().interactable = false;
        }
        else {
            displays["button1"].transform.GetChild(0).gameObject.SetActive(true);
            displays["button1"].GetComponent<Button>().interactable = true;
        }
    }

    IEnumerator MoveInDirection(int dir) {
        currMenuOption += dir;
        yield return new WaitForSecondsRealtime(0.5f);
        displays[menu[currMenuOption]].GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Select()
    {
        switch (currMenuOption) {
            default:
                Debug.LogError("Unrecognized menu option " + currMenuOption);
                break;
        }
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
            activeDisplayChange = DrawMainMenu();
            StartCoroutine(activeDisplayChange);
        }
    }

    private IEnumerator HideMenu(GameObject g) {
        SetOff();
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
        displays["time"].GetComponent<TextMeshPro>().text = currTime.Hour + "." + percentDisplay;
    }
}
