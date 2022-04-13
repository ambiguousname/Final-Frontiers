using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : ACESMenu
{
    public override IEnumerator Draw()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        yield return null;
    }

    public override void SetOff()
    {
        
        return;
    }

    public override void ButtonsCallback(int number)
    {
        return;
    }
}
