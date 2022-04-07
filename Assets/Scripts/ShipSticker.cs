using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSticker : MonoBehaviour
{
    GameObject _ship;
    Vector3 shipPos;
    Vector3 currOffset;
    // Start is called before the first frame update
    void Start()
    {
        _ship = GameObject.Find("ShipPrefab");
        shipPos = _ship.transform.position;
        currOffset = this.transform.position - shipPos;
    }

    public void MoveUpdate(Vector3 move) {
        currOffset = move + this.transform.position - shipPos;
    }

    
    void LateUpdate()
    {
        Vector3 newPos = _ship.transform.position + currOffset;
        this.transform.position = Helper.RotateAroundPivot(newPos, _ship.transform.position, _ship.transform.eulerAngles);
    }
}
