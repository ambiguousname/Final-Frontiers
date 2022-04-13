using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataBlockController : UpDownMenu
{
    protected struct DataBlock {
        public GameObject header;
        public string text;
    }

    public GameObject dataBlockPrefab;
    List<DataBlock> dataBlocks;
    GameObject blockList;
    TextMeshPro blockView;
    Button[] _buttons;

    public void InitController(Button[] buttons) {
        _buttons = buttons;
        SetUp(buttons, Color.white, Color.black);
        blockList = gameObject.FindChildWithName("BlockSelection");
        blockView = gameObject.FindChildWithName("BlockViewText").GetComponent<TextMeshPro>();
        dataBlocks = new List<DataBlock>();
    }

    public override IEnumerator Draw() {
        gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        _buttons[1].GetComponentInChildren<Text>().text = "";
        _buttons[4].GetComponentInChildren<Text>().text = "Pg. Dn";
        _buttons[1].interactable = false;
        _buttons[4].interactable = true;
        StartCoroutine(base.Draw());
    }

    public override void SetOff()
    {
        _buttons[1].GetComponentInChildren<Text>().text = "";
        _buttons[4].GetComponentInChildren<Text>().text = "";
        _buttons[1].interactable = false;
        _buttons[4].interactable = false;
        base.SetOff();
        gameObject.SetActive(false);
    }

    public void AddDataBlock(TextAsset data) {
        var dataBlock = new DataBlock();
        dataBlock.header = Instantiate(dataBlockPrefab, blockList.transform);
        var text = dataBlock.header.GetComponentInChildren<TextMeshPro>();
        text.text = data.name;
        dataBlock.text = data.text;
        dataBlocks.Add(dataBlock);
        if (dataBlocks.Count == 1) {
            dataBlock.header.GetComponent<SpriteRenderer>().color = Color.white;
            text.color = Color.black;
            blockView.text = data.text;
        }
        AddMenuOption(dataBlock.header);
    }

    private void ResetMove() {
        blockView.pageToDisplay = 0;
        blockView.text = dataBlocks[_currentlySelected].text;

        _buttons[1].GetComponentInChildren<Text>().text = "";
        _buttons[1].interactable = false;
    }

    protected new void MoveUp() {
        base.MoveUp();
        ResetMove();
    }

    protected new void MoveDown() {
        base.MoveDown();
        ResetMove();
    }

    public override void ButtonsCallback(int buttonNumber) {
        if (buttonNumber == 2)
        {
            _buttons[4].GetComponentInChildren<Text>().text = "Pg. Dn";
            _buttons[4].interactable = true;
            if (blockView.pageToDisplay > 1)
            {
                blockView.pageToDisplay -= 1;
            }
            if (blockView.pageToDisplay == 1)
            {
                _buttons[1].GetComponentInChildren<Text>().text = "";
                _buttons[1].interactable = false;
            }
        }
        if (buttonNumber == 5)
        {
            _buttons[1].GetComponentInChildren<Text>().text = "Pg. Up";
            _buttons[1].interactable = true;
            if (blockView.pageToDisplay < blockView.textInfo.pageCount)
            {
                blockView.pageToDisplay += 1;
            }
            if (blockView.pageToDisplay == blockView.textInfo.pageCount)
            {
                _buttons[4].GetComponentInChildren<Text>().text = "";
                _buttons[4].interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
