using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class ChangeControls : MonoBehaviour
{

    [FormerlySerializedAs("buttons")] [SerializeField]
    private List<Button> _buttons = new List<Button>();

    private InputAction[] _actionArray;
    private Button _currentButton;
    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).name.Contains("Default"))
            {
                _buttons.Add(transform.GetChild(i).GetComponent<Button>());
            }

        }
        _actionArray = PlayerInputHandler.Instance.PlayerInputActionMap.ToArray();
        var rebinds = PlayerPrefs.GetString("rebinds");
        foreach (var action in _actionArray)
        {
            action.LoadBindingOverridesFromJson(rebinds);
            Debug.Log(action.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions));
        }

        for (int i = 1; i < _buttons.Count + 1; i++)
        {
            _buttons[i - 1].GetComponentInChildren<TMP_Text>().text = _actionArray[i].GetBindingDisplayString();
        }
    }
    public void ChangeControl()
    {
        _currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Debug.Log(_currentButton);
        foreach (var button in _buttons) { button.interactable = false; }
        _currentButton.GetComponentInChildren<TMP_Text>().text = "Press any key";
        _actionArray[_buttons.IndexOf(_currentButton) + 1].Disable();
        _rebindOperation = _actionArray[_buttons.IndexOf(_currentButton) + 1].PerformInteractiveRebinding().WithControlsExcluding("Mouse").WithCancelingThrough("<Keyboard>/escape").OnMatchWaitForAnother(0.2f).Start();


    }
    public void DefaultButton()
    {
        foreach (var action in _actionArray)
        {
            action.RemoveAllBindingOverrides();
            action.SaveBindingOverridesAsJson();

        }
        for (int i = 1; i < _buttons.Count + 1; i++)
        {
            _buttons[i - 1].GetComponentInChildren<TMP_Text>().text = _actionArray[i].GetBindingDisplayString();
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (_rebindOperation != null)
        {
            if (_rebindOperation.completed || _rebindOperation.canceled)
            {
                _actionArray[_buttons.IndexOf(_currentButton) + 1].Enable();
                var rebinds = _actionArray[_buttons.IndexOf(_currentButton) + 1].SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("rebinds", rebinds);
                Debug.Log(_actionArray[_buttons.IndexOf(_currentButton) + 1].GetBindingDisplayString());
                _currentButton.GetComponentInChildren<TMP_Text>().text = _actionArray[_buttons.IndexOf(_currentButton) + 1].GetBindingDisplayString();
                foreach (var button in _buttons) { button.interactable = true; }
                _rebindOperation = null;
                _currentButton = null;

            }
        }

    }
}
