using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PassiveInteractionIndicator : MonoBehaviour
{
    public Image actionIndicator;
    public int actionIndex = 0;
    void Start()
    {
        if (Gamepad.current == null || Keyboard.current.lastUpdateTime < Gamepad.current.lastUpdateTime || Mouse.current.lastUpdateTime < Gamepad.current.lastUpdateTime)
            actionIndicator.sprite = InputSprites.instance.keyboardIndicators[actionIndex];
        else
            actionIndicator.sprite = InputSprites.instance.gamepadIndicators[actionIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Gamepad.current == null || Keyboard.current.lastUpdateTime < Gamepad.current.lastUpdateTime || Mouse.current.lastUpdateTime < Gamepad.current.lastUpdateTime)
            actionIndicator.sprite = InputSprites.instance.keyboardIndicators[actionIndex];
        else
            actionIndicator.sprite = InputSprites.instance.gamepadIndicators[actionIndex];
    }

}
