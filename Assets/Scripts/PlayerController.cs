using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    private DialogueCreator creator;
    private GameObject _camera;
    private float targetPitch;
    private Vector2 _move;
    Vector3 shipPos;
    Vector3 currRot;
    GameObject ship;

    private void Start()
    {
        _camera = gameObject.FindChildWithTag("MainCamera");
        creator = GameObject.Find("DialogueManager").GetComponent<DialogueCreator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetPitch = _camera.transform.localEulerAngles.x;
        ship = GameObject.Find("ShipPrefab");
        shipPos = ship.transform.position;
        currRot = ship.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<ShipSticker>().MoveUpdate(this.transform.position - currPos);
        //this.transform.localPosition += (_camera.transform.forward * _move.y + _camera.transform.right * _move.x) * Time.deltaTime;

    }

    private void LateUpdate()
    {
        Vector3 dist = ship.transform.position - shipPos;
        Vector3 rotDist = ship.transform.eulerAngles - currRot;
        Vector3 playerPos = Helper.RotateAroundPivot(this.transform.position + dist, ship.transform.position, rotDist);
        GetComponent<CharacterController>().Move(playerPos - this.transform.position);
        this.transform.rotation = Quaternion.Euler(ship.transform.eulerAngles);
        shipPos = ship.transform.position;
        currRot = ship.transform.eulerAngles;
    }

    // From StarterAssets and FirstPersonController.cs
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void OnLook(InputValue value) {
        var toRot = value.Get<Vector2>();
        targetPitch += toRot.y * rotationSpeed * Time.deltaTime;
        targetPitch = ClampAngle(targetPitch, -90.0f, 90.0f);
        Vector3 fullRot = new Vector3(targetPitch, _camera.transform.localEulerAngles.y + (toRot.x * Time.deltaTime * rotationSpeed), _camera.transform.localEulerAngles.z);
        _camera.transform.localRotation = Quaternion.Euler(fullRot);
    }

    void OnMove(InputValue value) {
        var toMove = value.Get<Vector2>();
        _move = toMove;
    }

    void OnOne(InputValue value)
    {
        creator.PressNumButton(1);
    }

    void OnTwo()
    {
        creator.PressNumButton(2);
    }

    void OnThree()
    {
        creator.PressNumButton(3);
    }

    void OnFour()
    {
        creator.PressNumButton(4);
    }

    void OnFive()
    {
        creator.PressNumButton(5);
    }

    void OnSix()
    {
        creator.PressNumButton(6);
    }

    void OnSeven()
    {
        creator.PressNumButton(7);
    }

    void OnEight()
    {
        creator.PressNumButton(8);
    }
}
