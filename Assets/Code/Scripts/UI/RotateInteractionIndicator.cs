using UnityEngine;

public class RotateInteractionIndicator : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform boxContent;

    [Header("Flip Config")]
    [SerializeField] private float flipAngle;
    private bool isFlipped = false;

    private bool playerIsNear;
    private Transform cameraOrientation;

    private GameObject interactionIndicatorUI;
    private bool isFlipping = true;

    private void Start()
    {
        interactionIndicatorUI = transform.GetChild(0).gameObject;

        if (interactionIndicatorUI == null)
        {
            Debug.Log(gameObject.name + " should have an interaction indicator, but it is missing a necessary component! Interaction indicator shall be disabled.");
            enabled = false;
        }

        else
        {
            interactionIndicatorUI.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (playerIsNear)
        {
            DisableInteractionIndicator();
        }
    }

    private void Update()
    {
        if (playerIsNear)
        {
            TargetInteractionIndicatorTowardsPlayer();
        }
    }

    private void TargetInteractionIndicatorTowardsPlayer()
    {
        Vector3 directionToPlayer = new Vector3(cameraOrientation.transform.position.x - interactionIndicatorUI.transform.position.x, 0, cameraOrientation.transform.position.z - interactionIndicatorUI.transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        interactionIndicatorUI.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        // if rotation is over set value, flip to the other pivot point
        if (interactionIndicatorUI.transform.eulerAngles.y > flipAngle && interactionIndicatorUI.transform.eulerAngles.y < 180f && !isFlipped)
        {
            isFlipped = true;
            FlipToOtherPivotPoint();
        }

        else if ((interactionIndicatorUI.transform.eulerAngles.y <= flipAngle || interactionIndicatorUI.transform.eulerAngles.y >= 180f) && isFlipped)
        {
            isFlipped = false;
            FlipToOtherPivotPoint();
        }
    }

    public void EnableInteractionIndicator(Transform orientation)
    {
        playerIsNear = true;
        cameraOrientation = orientation;

        interactionIndicatorUI.SetActive(true);
    }

    public void DisableInteractionIndicator()
    {
        playerIsNear = false;
        interactionIndicatorUI.SetActive(false);
    }

    private void FlipToOtherPivotPoint()
    {
        if (isFlipping)
        {
            return;
        }

        isFlipping = true;
        Invoke(nameof(EnableFlipping), 0.5f);

        // change pivot point location
        Vector3 newPivotPosition = pivotPoint.localPosition;
        newPivotPosition.x *= -1f;
        pivotPoint.localPosition = newPivotPosition;

        // change canvas transform settings
        Vector3 newCanvasPosition = canvas.localPosition;
        newCanvasPosition.x *= -1f;
        canvas.localPosition = newCanvasPosition;

        if (canvas.localRotation.y != 0)
        {
            canvas.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            canvas.localRotation = Quaternion.Euler(0, 180, 0);
        }

        // flip the content in the UI box
        if (boxContent.localRotation.y != 0)
        {
            boxContent.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            boxContent.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void EnableFlipping()
    {
        isFlipping = false;
    }
}
