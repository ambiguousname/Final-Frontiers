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
    Button _pageUp;
    Button _pageDown;
    Button _exit;

    public void InitController(GameObject upButton, GameObject downButton, Button pageUp, Button pageDown, Button exit) {
        SetUp(upButton, downButton, Color.white, Color.black);
        _pageUp = pageUp;
        _pageDown = pageDown;
        _exit = exit;
        blockList = gameObject.FindChildWithName("BlockSelection");
        blockView = gameObject.FindChildWithName("BlockViewText").GetComponent<TextMeshPro>();
        dataBlocks = new List<GameObject>();
    }

    public override IEnumerator Draw() {
        base.Draw();
        blockView.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        _exit.GetComponentInChildren<Text>().text = "Back";
        _pageUp.GetComponentInChildren<Text>().text = "Pg. Up";
        _pageDown.GetComponentInChildren<Text>().text = "Pg. Down";
        _pageDown.interactable = true;
        _pageUp.interactable = true;
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
            _pageDown.transform.GetChild(0).gameObject.SetActive(true);
            _pageDown.interactable = true;
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
            _pageUp.transform.GetChild(0).gameObject.SetActive(true);
            _pageUp.interactable = true;
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
