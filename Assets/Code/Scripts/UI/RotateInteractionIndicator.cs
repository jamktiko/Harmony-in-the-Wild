using UnityEngine;
using UnityEngine.Serialization;

public class RotateInteractionIndicator : MonoBehaviour
{
    [FormerlySerializedAs("pivotPoint")]
    [Header("Needed References")]
    [SerializeField] private Transform _pivotPoint;
    [FormerlySerializedAs("canvas")] [SerializeField] private RectTransform _canvas;
    [FormerlySerializedAs("boxContent")] [SerializeField] private RectTransform _boxContent;

    [FormerlySerializedAs("flipAngle")]
    [Header("Flip Config")]
    [SerializeField] private float _flipAngle;
    private bool _isFlipped = false;

    private bool _playerIsNear;
    private Transform _cameraOrientation;

    private GameObject _interactionIndicatorUI;
    private bool _isFlipping = true;

    private void Start()
    {
        _interactionIndicatorUI = transform.GetChild(0).gameObject;

        if (_interactionIndicatorUI == null)
        {
            Debug.Log(gameObject.name + " should have an interaction indicator, but it is missing a necessary component! Interaction indicator shall be disabled.");
            enabled = false;
        }

        else
        {
            _interactionIndicatorUI.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (_playerIsNear)
        {
            DisableInteractionIndicator();
        }
    }

    private void Update()
    {
        if (_playerIsNear)
        {
            TargetInteractionIndicatorTowardsPlayer();
        }
    }

    private void TargetInteractionIndicatorTowardsPlayer()
    {
        Vector3 directionToPlayer = new Vector3(_cameraOrientation.transform.position.x - _interactionIndicatorUI.transform.position.x, 0, _cameraOrientation.transform.position.z - _interactionIndicatorUI.transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        _interactionIndicatorUI.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        // if rotation is over set value, flip to the other pivot point
        if (_interactionIndicatorUI.transform.eulerAngles.y > _flipAngle && _interactionIndicatorUI.transform.eulerAngles.y < 180f && !_isFlipped)
        {
            _isFlipped = true;
            FlipToOtherPivotPoint();
        }

        else if ((_interactionIndicatorUI.transform.eulerAngles.y <= _flipAngle || _interactionIndicatorUI.transform.eulerAngles.y >= 180f) && _isFlipped)
        {
            _isFlipped = false;
            FlipToOtherPivotPoint();
        }
    }

    public void EnableInteractionIndicator(Transform orientation)
    {
        _playerIsNear = true;
        _cameraOrientation = orientation;

        _interactionIndicatorUI.SetActive(true);
    }

    public void DisableInteractionIndicator()
    {
        _playerIsNear = false;
        _interactionIndicatorUI.SetActive(false);
    }

    private void FlipToOtherPivotPoint()
    {
        if (_isFlipping)
        {
            return;
        }

        _isFlipping = true;
        Invoke(nameof(EnableFlipping), 0.5f);

        // change pivot point location
        Vector3 newPivotPosition = _pivotPoint.localPosition;
        newPivotPosition.x *= -1f;
        _pivotPoint.localPosition = newPivotPosition;

        // change canvas transform settings
        Vector3 newCanvasPosition = _canvas.localPosition;
        newCanvasPosition.x *= -1f;
        _canvas.localPosition = newCanvasPosition;

        if (_canvas.localRotation.y != 0)
        {
            _canvas.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            _canvas.localRotation = Quaternion.Euler(0, 180, 0);
        }

        // flip the content in the UI box
        if (_boxContent.localRotation.y != 0)
        {
            _boxContent.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            _boxContent.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void EnableFlipping()
    {
        _isFlipping = false;
    }
}
