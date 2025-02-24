using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeadButt : MonoBehaviour
{
    float force;
    [SerializeField]float finalForce;
    [SerializeField]Slider Slider;
    [SerializeField] Rigidbody currentRock;
    void FixedUpdate()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && force != 0)
        {
            TriggerHeadButt();
        }
        else if (Mouse.current.leftButton.isPressed&&force<10)
        {
            force += 0.05f;
            Slider.value = force;
            Vector3 direction = currentRock.position-FoxMovement.instance.foxFront.position ;
            Debug.DrawLine(currentRock.position, direction * 100, Color.green);
        }
        else
        {
            Slider.value -=0.2f;
            force = Slider.value;
        }
    }
    private void TriggerHeadButt() 
    {
        finalForce = force;
        FoxMovement.instance.playerAnimator.Play("PL_Smashing_ANI");
        if (currentRock!=null)
        {
            Vector3 direction = currentRock.position - FoxMovement.instance.foxFront.position;
            currentRock.AddForce(direction*finalForce*10,ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            currentRock = other.GetComponent<Rigidbody>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
        currentRock=null;
    }

}
