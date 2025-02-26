using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputSprites : MonoBehaviour
{
    public static InputSprites Instance;

    // CONTROLS //
    // E / B: Interact
    // F / X: Ability
    // Space / A: Glide
    // Left Shift / Right Trigger: Sprint
    // Left Ctrl / Left Trigger: Snowdive
    // M / Touchpad / View: Map
    [FormerlySerializedAs("keyboardIndicators")] public Sprite[] KeyboardIndicators;
    [FormerlySerializedAs("gamepadIndicators")] public Sprite[] GamepadIndicators;

    public Dictionary<string, KeyDeviceSetup> KeySetups = new Dictionary<string, KeyDeviceSetup>();

    private void Awake()
    {
        Instance = this;
        KeySetups.Add("E", new KeyDeviceSetup("E", "B"));
        KeySetups.Add("F", new KeyDeviceSetup("F", "X"));
        KeySetups.Add("SPACE", new KeyDeviceSetup("SPACE", "A"));
        KeySetups.Add("LEFT SHIFT", new KeyDeviceSetup("LEFT SHIFT", "RIGHT TRIGGER"));
        KeySetups.Add("LEFT CTRL", new KeyDeviceSetup("LEFT CTRL", "LEFT TRIGGER"));
        KeySetups.Add("M", new KeyDeviceSetup("M", "VIEW/TOUCHPAD"));
    }

}

public class KeyDeviceSetup
{
    public string Keyboard;
    public string Gamepad;

    public KeyDeviceSetup(string keyboard, string gamepad)
    {
        this.Keyboard = keyboard;
        this.Gamepad = gamepad;
    }
}
