using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class ToggleInteractionIndicator : MonoBehaviour
{
    public Image ActionIndicator;
    public int actionIndex = 0;

    private RotateInteractionIndicator _rotationComponent;

    private void Start()
    {
        _rotationComponent = GetComponent<RotateInteractionIndicator>();

        if (_rotationComponent == null)
        {
            Debug.Log(gameObject.name + " doesn't have RotateInteractionIndicator component which is mandatory for interactable objects. Interaction will be disabled.");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            // locate the player camera and enable interaction indicator
            GameObject camera = other.transform.parent.Find("FreeLook Camera").gameObject;

            if (camera != null)
            {
                Debug.Log("Interaction indicator enabled!");
                _rotationComponent.EnableInteractionIndicator(camera.transform);
            }

            else
            {
                Debug.Log("No camera located for the interaction indicator!");
            }

            if (ActionIndicator != null)
            {
                if (Gamepad.current == null || Keyboard.current.lastUpdateTime > Gamepad.current.lastUpdateTime || Mouse.current.lastUpdateTime > Gamepad.current.lastUpdateTime)
                    ActionIndicator.sprite = InputSprites.instance.keyboardIndicators[actionIndex];
                else
                    ActionIndicator.sprite = InputSprites.instance.gamepadIndicators[actionIndex];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _rotationComponent.DisableInteractionIndicator();
        }
    }
}
