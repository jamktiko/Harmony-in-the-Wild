using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelToggle : MonoBehaviour
{
    [SerializeField] private GameObject redFox;
    [SerializeField] private GameObject arcticFox;
    [SerializeField] private GameObject changeEffect;
    [SerializeField] private CameraMovement playerCamera;

    private Vector3 effectPosition;
    private Animator currentAnimator;

    public void TogglePlayerModel()
    {
        effectPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Instantiate(changeEffect, effectPosition, Quaternion.identity);
        effectPosition = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        Instantiate(changeEffect, effectPosition, Quaternion.identity);
        effectPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        Instantiate(changeEffect, effectPosition, Quaternion.identity);

        if (redFox.activeInHierarchy)
        {
            redFox.SetActive(false);
            arcticFox.SetActive(true);

            currentAnimator = arcticFox.GetComponent<Animator>();
            GetComponent<FoxMovement>().playerAnimator = currentAnimator;
            playerCamera.foxObject = arcticFox.transform;
        }

        else
        {
            redFox.SetActive(true);
            arcticFox.SetActive(false);

            currentAnimator = redFox.GetComponent<Animator>();
            GetComponent<FoxMovement>().playerAnimator = currentAnimator;
            playerCamera.foxObject = redFox.transform;
        }
    }
}
