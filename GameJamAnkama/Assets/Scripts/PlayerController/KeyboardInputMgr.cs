using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputMgr : InputManager
{
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Debug.Log(direction);
        float angle = Vector3.SignedAngle(Vector3.forward, direction, this.transform.up);
        _playerController.Move(angle, !direction.Equals(Vector3.zero));
        if (Input.GetButtonUp("Jump"))
            _playerController.Eat();
        if (Input.GetButtonUp("Submit"))
            _playerController.Dash();
    }
}
