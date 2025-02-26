using System.Collections.Generic;
using UnityEngine;

public class InputSprites : MonoBehaviour
{
    public static InputSprites instance;

    // CONTROLS //
    // E / B: Interact
    // F / X: Ability
    // Space / A: Glide
    // Left Shift / Right Trigger: Sprint
    // Left Ctrl / Left Trigger: Snowdive
    // M / Touchpad / View: Map
    public Sprite[] keyboardIndicators;
    public Sprite[] gamepadIndicators;

    public Dictionary<string, KeyDeviceSetup> keySetups = new Dictionary<string, KeyDeviceSetup>();

    private void Awake()
    {
        instance = this;
        keySetups.Add("E", new KeyDeviceSetup("E", "B"));
        keySetups.Add("F", new KeyDeviceSetup("F", "X"));
        keySetups.Add("SPACE", new KeyDeviceSetup("SPACE", "A"));
        keySetups.Add("LEFT SHIFT", new KeyDeviceSetup("LEFT SHIFT", "RIGHT TRIGGER"));
        keySetups.Add("LEFT CTRL", new KeyDeviceSetup("LEFT CTRL", "LEFT TRIGGER"));
        keySetups.Add("M", new KeyDeviceSetup("M", "VIEW/TOUCHPAD"));
    }

}

public class KeyDeviceSetup
{
    public string keyboard;
    public string gamepad;

    public KeyDeviceSetup(string keyboard, string gamepad)
    {
        this.keyboard = keyboard;
        this.gamepad = gamepad;
    }
}
