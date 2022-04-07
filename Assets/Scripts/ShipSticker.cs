using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSticker : MonoBehaviour
{
    GameObject _ship;
    Vector3 currOffset;
    // Start is called before the first frame update
    void Start()
    {
        _ship = GameObject.Find("ShipPrefab");
        currOffset = this.transform.position - _ship.transform.position;
    }

    public void MoveUpdate(Vector3 move) {
        currOffset += move * 10;
    }

    
    void LateUpdate()
    {
        Vector3 newPos = _ship.transform.position + currOffset;
        this.transform.position = Helper.RotateAroundPivot(newPos, _ship.transform.position, _ship.transform.eulerAngles);
        this.transform.rotation = Quaternion.Euler(_ship.transform.eulerAngles);
    }
}
