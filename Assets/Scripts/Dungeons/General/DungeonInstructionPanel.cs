using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonInstructionPanel : MonoBehaviour
{
    public PlayerInput PlayerInput;

    private void Start()
    {
        PlayerInput = FindObjectOfType<PlayerInput>();
    }
    public void CloseInfo(InputAction.CallbackContext context) 
    {
        if (context.performed&&gameObject.activeSelf|| context.performed&&!DialogueManager.instance.isDialoguePlaying)
        {
            PlayerInput.SwitchCurrentActionMap("Player");
            Debug.Log("yo bro");
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
