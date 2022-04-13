using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataBlockController : UpDownMenu
{
    public GameObject dataBlockPrefab;
    List<GameObject> dataBlocks;
    GameObject blockList;
    TextMeshPro blockView;
    Button[] _buttons;

    public void InitController(Button[] buttons) {
        _buttons = buttons;
        SetUp(buttons, Color.white, Color.black);
        blockList = gameObject.FindChildWithName("BlockSelection");
        blockView = gameObject.FindChildWithName("BlockViewText").GetComponent<TextMeshPro>();
        dataBlocks = new List<GameObject>();
    }

    public override IEnumerator Draw() {
        gameObject.SetActive(true);
        base.Draw();
        blockView.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        _buttons[1].GetComponentInChildren<Text>().text = "Pg. Up";
        _buttons[4].GetComponentInChildren<Text>().text = "Pg. Down";
        _buttons[1].interactable = true;
        _buttons[4].interactable = true;
    }

    public override void SetOff()
    {
        _buttons[1].GetComponentInChildren<Text>().text = "";
        _buttons[4].GetComponentInChildren<Text>().text = "";
        _buttons[1].interactable = false;
        _buttons[4].interactable = false;
        blockView.gameObject.SetActive(false);
        base.SetOff();
        gameObject.SetActive(false);
    }

    public void AddDataBlock(TextAsset data) {
        var dataBlock = Instantiate(dataBlockPrefab, blockList.transform);
        var text = dataBlock.GetComponentInChildren<TextMeshPro>();
        text.text = data.name;
        dataBlocks.Add(dataBlock);
        if (dataBlocks.Count == 1) {
            dataBlock.GetComponent<SpriteRenderer>().color = Color.white;
            text.color = Color.black;
        }
        AddMenuOption(dataBlock);
    }

    public void ButtonsCallback(int buttonNumber, Button buttonObject) {
        if (buttonNumber == 2)
        {
            _buttons[4].transform.GetChild(0).gameObject.SetActive(true);
            _buttons[4].interactable = true;
            if (blockView.pageToDisplay > 0)
            {
                blockView.pageToDisplay -= 1;
            }
            if (blockView.pageToDisplay == 0)
            {
                buttonObject.transform.GetChild(0).gameObject.SetActive(false);
                buttonObject.interactable = false;
            }
        }
        if (buttonNumber == 5)
        {
            _buttons[1].transform.GetChild(0).gameObject.SetActive(true);
            _buttons[1].interactable = true;
            if (blockView.pageToDisplay < blockView.textInfo.pageCount)
            {
                blockView.pageToDisplay += 1;
            }
            if (blockView.pageToDisplay == blockView.textInfo.pageCount)
            {
                buttonObject.transform.GetChild(0).gameObject.SetActive(false);
                buttonObject.interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
