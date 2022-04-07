using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSticker : MonoBehaviour
{
    GameObject _ship;
    Vector3 shipPos;
    Vector3 currOffset;
    Vector3 currRot;
    // Start is called before the first frame update
    void Start()
    {
        _ship = GameObject.Find("ShipPrefab");
        shipPos = _ship.transform.position;
        currOffset = this.transform.position - shipPos;
        currRot = this.transform.eulerAngles - _ship.transform.eulerAngles;
    }

    public void MoveUpdate(Vector3 move) {
        currOffset += move * 10;
    }

    public void RotateUpdate(Vector3 rot) {
        currRot += rot;
    }

    
    void LateUpdate()
    {
        Vector3 newPos = _ship.transform.position + currOffset;
        this.transform.position = Helper.RotateAroundPivot(newPos, _ship.transform.position, _ship.transform.eulerAngles);
        this.transform.rotation = Quaternion.Euler(currRot + _ship.transform.eulerAngles);
    }
}
