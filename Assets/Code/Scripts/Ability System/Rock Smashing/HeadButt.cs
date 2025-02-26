using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeadButt : MonoBehaviour
{
    private float _force;
    [SerializeField] private float _finalForce;
    [SerializeField] private Slider _slider;
    [SerializeField] private Rigidbody _currentRock;
    void FixedUpdate()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && _force != 0)
        {
            TriggerHeadButt();
        }
        else if (Mouse.current.leftButton.isPressed && _force < 10)
        {
            _force += 0.05f;
            _slider.value = _force;
            Vector3 direction = _currentRock.position - FoxMovement.Instance.FoxFront.position;
            Debug.DrawLine(_currentRock.position, direction * 100, Color.green);
        }
        else
        {
            _slider.value -= 0.2f;
            _force = _slider.value;
        }
    }
    private void TriggerHeadButt()
    {
        _finalForce = _force;
        FoxMovement.Instance.PlayerAnimator.Play("PL_Smashing_ANI");
        if (_currentRock != null)
        {
            Vector3 direction = _currentRock.position - FoxMovement.Instance.FoxFront.position;
            _currentRock.AddForce(direction * _finalForce * 10, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            _currentRock = other.GetComponent<Rigidbody>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
            _currentRock = null;
    }

}
