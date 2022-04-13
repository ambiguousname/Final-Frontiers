using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACESMenu : MonoBehaviour
{
    public abstract void ButtonsCallback(int number);

    public abstract IEnumerator SetOff();

    public abstract IEnumerator Draw();
}
