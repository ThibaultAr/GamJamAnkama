using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class KeyboardInputMgr : InputManager
{
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    // Update is called once per frame
    void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        Vector3 direction = new Vector3(state.ThumbSticks.Left.X, 0, state.ThumbSticks.Left.Y);
        float angle = Vector3.SignedAngle(Vector3.forward, direction, this.transform.up);
        _playerController.Move(angle, !direction.Equals(Vector3.zero));

        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
            _playerController.Eat();
        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
            _playerController.Dash();
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
            _playerController.Aim();
        if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
            _playerController.Shoot();
    }
}
