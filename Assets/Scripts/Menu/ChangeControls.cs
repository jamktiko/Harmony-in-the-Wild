using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class ChangeControls : MonoBehaviour
{
    
    [SerializeField] List<Button> buttons = new List<Button>();
    InputAction[] actionArray;
    Button currentButton;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).name.Contains("Default"))
            {
                buttons.Add(transform.GetChild(i).GetComponent<Button>());
            }
            
        }
        actionArray = PlayerInputHandler.instance.playerInputActionMap.ToArray();
        var rebinds = PlayerPrefs.GetString("rebinds");
        foreach (var action in actionArray) 
        {
            action.LoadBindingOverridesFromJson(rebinds);
            Debug.Log(action.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions));
        }

        for (int i = 1; i < buttons.Count+1; i++)
        {
            buttons[i-1].GetComponentInChildren<TMP_Text>().text = actionArray[i].GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);
        }
    }
    public void ChangeControl() 
    {
         currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Debug.Log(currentButton);
        foreach (var button in buttons) { button.interactable = false; }
        currentButton.GetComponentInChildren<TMP_Text>().text = "Press any key";
        actionArray[buttons.IndexOf(currentButton) + 1].Disable();
        rebindOperation = actionArray[buttons.IndexOf(currentButton)+1].PerformInteractiveRebinding().WithControlsExcluding("Mouse").WithCancelingThrough("<Keyboard>/escape").OnMatchWaitForAnother(0.2f).Start();
        
       
    }
    public void DefaultButton() 
    {
        foreach (var action in actionArray)
        {
            action.RemoveAllBindingOverrides();
            action.SaveBindingOverridesAsJson();

        }
        for (int i = 1; i < buttons.Count + 1; i++)
        {
            buttons[i - 1].GetComponentInChildren<TMP_Text>().text = actionArray[i].GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (rebindOperation!=null)
        {
            if (rebindOperation.completed||rebindOperation.canceled)
            {
                actionArray[buttons.IndexOf(currentButton) + 1].Enable();
                var rebinds = actionArray[buttons.IndexOf(currentButton) + 1].SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("rebinds", rebinds);
                Debug.Log(actionArray[buttons.IndexOf(currentButton) + 1].GetBindingDisplayString());
                currentButton.GetComponentInChildren<TMP_Text>().text = actionArray[buttons.IndexOf(currentButton) + 1].GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);
                foreach (var button in buttons) { button.interactable = true; }
                rebindOperation = null;
                currentButton = null;
                
            }
        }
        
    }
}
