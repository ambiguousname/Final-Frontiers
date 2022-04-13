using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : ACESMenu
{
    public override IEnumerator Draw()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    public override void SetOff()
    {
        gameObject.SetActive(false);
        return;
    }

    public override void ButtonsCallback(int number)
    {
        return;
    }
}
