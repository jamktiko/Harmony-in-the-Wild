using UnityEngine;

public class LilySocketTrigger : MonoBehaviour
{
    [SerializeField] private string correctColor;

    private bool isFilled;
    private string lilyKeyword = "Lily";
    private GameObject fillingObject;
    private Rigidbody fillingRigidbody;
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (other.name.Contains(lilyKeyword) && !isFilled && !rb.isKinematic)
        {
            isFilled = true;
            LilyPuzzle.instance.socketsFilled++;
            fillingObject = other.gameObject;
            fillingRigidbody = rb;

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
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (other.name.Contains(lilyKeyword) && isFilled)
        {
            isFilled = false;
            LilyPuzzle.instance.socketsFilled--;
            fillingObject = null;
            fillingRigidbody = null;

            if (other.name.Contains(correctColor))
            {
                LilyPuzzle.instance.CheckPuzzleProgress(-1);
            }
        }
    }
    private void Update()
    {
        if (fillingObject != null && !fillingRigidbody.isKinematic)
        {
            fillingObject.transform.position = transform.position;
            fillingRigidbody.velocity = Vector3.zero;
            fillingObject.transform.rotation = transform.rotation;
        }
    }
}