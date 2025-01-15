using System.Collections;
using UnityEngine;

public class PlayerModelToggle : MonoBehaviour
{
    public GameObject redFox;
    [SerializeField] private GameObject arcticFox;
    [SerializeField] private GameObject changeEffect;
    [SerializeField] private CameraMovement playerCamera;
    [SerializeField] private GameObject changeControl;
    [SerializeField] private ParticleSystem[] changePSSnow;
    [SerializeField] private ParticleSystem[] changePSAutumn;

    private Vector3 effectPosition;
    private Animator currentAnimator;

    private void Update()
    {
        if (PlayerInputHandler.instance.TogglePlayerModelInput.WasPressedThisFrame())
            TogglePlayerModelPublic();
    }

    public void TogglePlayerModelPublic(int model = 0)
    {
        StartCoroutine(TogglePlayerModel(model));
    }

    IEnumerator TogglePlayerModel(int model)
    {
        bool snow = false;
        if (model == 0 && !redFox.activeInHierarchy || model == 2)
            snow = true;
        ChangeVFX(snow);
        StartCoroutine(MaintainRotation(5));
        yield return new WaitForSeconds(3.5f);
        if (!snow)
        {
            //ChangeModelToForest();
        }
        else
        {
            //ChangeModelToArctic();
        }
    }

    IEnumerator MaintainRotation(float duration)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            if (redFox.activeInHierarchy)
                changeControl.transform.rotation = redFox.transform.rotation;
            else
                changeControl.transform.rotation = arcticFox.transform.rotation;
            yield return null;
        }
    }
    
    private void ChangeVFX(bool snow)
    {
        //effectPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        //Instantiate(changeEffect, effectPosition, Quaternion.identity);
        //effectPosition = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        //Instantiate(changeEffect, effectPosition, Quaternion.identity);
        //effectPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        //Instantiate(changeEffect, effectPosition, Quaternion.identity);
        if (snow)
            for (int i = 0; i < changePSSnow.Length; i++)
            {
                changePSSnow[i].Play();
            }
        else
            for (int i = 0; i < changePSAutumn.Length; i++)
            {
                changePSAutumn[i].Play();
            }
    }
    /*public void ChangeModelToForest()
    {
        if (!redFox.activeInHierarchy)
        {
            GameEventsManager.instance.playerEvents.ChangePlayerModel();

            ChangeVFX();

            redFox.SetActive(true);
            redFox.transform.rotation = arcticFox.transform.rotation;
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
            GameEventsManager.instance.playerEvents.ChangePlayerModel();

            ChangeVFX();

            redFox.SetActive(false);
            arcticFox.transform.rotation = redFox.transform.rotation;
            arcticFox.SetActive(true);

            currentAnimator = arcticFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = arcticFox.transform;
        }
    }*/

    public void ChangeModelTo(string modelName)
    {
        GameEventsManager.instance.playerEvents.ChangePlayerModel();

        if (modelName == "Arctic")
        {
            if (!arcticFox.activeInHierarchy)
            {
                ChangeVFX(true);

                redFox.SetActive(false);
                arcticFox.SetActive(true);

                currentAnimator = arcticFox.GetComponent<Animator>();
                FoxMovement.instance.playerAnimator = currentAnimator;
                playerCamera.foxObject = arcticFox.transform;
            }
        }

        else if(modelName == "Forest")
        {
            if (!redFox.activeInHierarchy)
            {
                ChangeVFX(false);

                redFox.SetActive(true);
                arcticFox.SetActive(false);

                currentAnimator = redFox.GetComponent<Animator>();
                FoxMovement.instance.playerAnimator = currentAnimator;
                playerCamera.foxObject = redFox.transform;
            }
        }
    }
}
