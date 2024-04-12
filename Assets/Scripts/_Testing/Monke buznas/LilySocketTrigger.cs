using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilySocketTrigger : MonoBehaviour
{
    [SerializeField] private string correctColor;

    private bool isFilled;
    private string lilyKeyword = "Lily";
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (other.name.Contains(lilyKeyword) && !isFilled && !rb.isKinematic)
        {
            isFilled = true;
            LilyPuzzle.instance.socketsFilled++;

            other.transform.SetParent(null);
            other.transform.position = transform.position;

            if (other.name.Contains(correctColor))
            {
                LilyPuzzle.instance.CheckPuzzleProgress(1);
            }
            else
            {
                LilyPuzzle.instance.CheckPuzzleProgress(0);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains(lilyKeyword) && isFilled)
        {
            isFilled = false;
            LilyPuzzle.instance.socketsFilled--;

            if (other.name.Contains(correctColor))
            {
                LilyPuzzle.instance.CheckPuzzleProgress(-1);
            }
        }
    }
}