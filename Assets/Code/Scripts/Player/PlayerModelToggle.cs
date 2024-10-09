using UnityEngine;

public class PlayerModelToggle : MonoBehaviour
{
    public GameObject redFox;
    [SerializeField] private GameObject arcticFox;
    [SerializeField] private GameObject changeEffect;
    [SerializeField] private CameraMovement playerCamera;

    private Vector3 effectPosition;
    private Animator currentAnimator;

    private void Update()
    {
        //if (PlayerInputHandler.instance.TogglePlayerModelInput.WasPressedThisFrame()) 
        //    TogglePlayerModel();
    }
    private void TogglePlayerModel()
    {
        ChangeVFX();

        if (!redFox.activeInHierarchy)
        {
            ChangeModelToForest();
        }

        else
        {
            ChangeModelToArctic();
        }
    }
    private void ChangeVFX()
    {
        effectPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Instantiate(changeEffect, effectPosition, Quaternion.identity);
        effectPosition = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        Instantiate(changeEffect, effectPosition, Quaternion.identity);
        effectPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        Instantiate(changeEffect, effectPosition, Quaternion.identity);
    }
    public void ChangeModelToForest()
    {
        if (!redFox.activeInHierarchy)
        {
            ChangeVFX();

            redFox.SetActive(true);
            arcticFox.SetActive(false);

            currentAnimator = redFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = redFox.transform;
        }
    }
    public void ChangeModelToArctic()
    {
        if (redFox.activeInHierarchy)
        {
            ChangeVFX();

            redFox.SetActive(false);
            arcticFox.SetActive(true);

            currentAnimator = arcticFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = arcticFox.transform;
        }
    }

}
