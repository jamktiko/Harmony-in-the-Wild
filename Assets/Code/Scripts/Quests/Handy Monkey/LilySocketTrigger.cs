using UnityEngine;
using UnityEngine.Serialization;

public class LilySocketTrigger : MonoBehaviour
{
    [FormerlySerializedAs("correctColor")] [SerializeField] private string _correctColor;

    private bool _isFilled;
    private string _lilyKeyword = "Lily";
    private GameObject _fillingObject;
    private Rigidbody _fillingRigidbody;
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (other.name.Contains(_lilyKeyword) && !_isFilled && !rb.isKinematic)
        {
            _isFilled = true;
            LilyPuzzle.Instance.SocketsFilled++;
            _fillingObject = other.gameObject;
            _fillingRigidbody = rb;

            other.transform.SetParent(null);
            other.transform.position = transform.position;

            if (other.name.Contains(_correctColor))
            {
                LilyPuzzle.Instance.CheckPuzzleProgress(1);
            }
            else
            {
                LilyPuzzle.Instance.CheckPuzzleProgress(0);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (other.name.Contains(_lilyKeyword) && _isFilled)
        {
            _isFilled = false;
            LilyPuzzle.Instance.SocketsFilled--;
            _fillingObject = null;
            _fillingRigidbody = null;

            if (other.name.Contains(_correctColor))
            {
                LilyPuzzle.Instance.CheckPuzzleProgress(-1);
            }
        }
    }
    private void Update()
    {
        if (_fillingObject != null && !_fillingRigidbody.isKinematic)
        {
            _fillingObject.transform.position = transform.position;
            _fillingRigidbody.velocity = Vector3.zero;
            _fillingObject.transform.rotation = transform.rotation;
        }
    }
}