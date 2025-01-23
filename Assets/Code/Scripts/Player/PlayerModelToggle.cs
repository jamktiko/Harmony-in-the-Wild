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

    private Animator currentAnimator;

    private bool canTriggerVFX = false; // this bool is enabled a bit after the scene is loaded; the point is to prevent VFX being triggered when entering the scene

    private void Start()
    {
        Invoke(nameof(EnableVFX), 2f);
    }

    private void Update()
    {
        //if (PlayerInputHandler.instance.TogglePlayerModelInput.WasPressedThisFrame())
        //    TogglePlayerModelPublic();
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
        yield return null;
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

        StartCoroutine(ChangeModel(snow, 3.3f));
    }

    private IEnumerator ChangeModel(bool toArcticFox, float timeBeforeChange)
    {
        yield return new WaitForSeconds(timeBeforeChange);

        if (toArcticFox)
        {
            redFox.SetActive(false);
            arcticFox.SetActive(true);

            currentAnimator = arcticFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = arcticFox.transform;
        }

        else
        {
            redFox.SetActive(true);
            arcticFox.SetActive(false);

            currentAnimator = redFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = redFox.transform;
        }
    }

    public void PrepareForModelChange(string modelName)
    {
        if (modelName == "Arctic")
        {
            if (!arcticFox.activeInHierarchy)
            {
                if (canTriggerVFX)
                {
                    ChangeVFX(true);
                    StartCoroutine(MaintainRotation(5));
                }

                else
                {
                    StartCoroutine(ChangeModel(true, 0f));
                }
            }
        }

        else if (modelName == "Forest")
        {
            if (!redFox.activeInHierarchy)
            {
                if (canTriggerVFX)
                {
                    ChangeVFX(false);
                    StartCoroutine(MaintainRotation(5));
                }

                else
                {
                    StartCoroutine(ChangeModel(false, 0f));
                }
            }
        }
    }

    private void EnableVFX()
    {
        canTriggerVFX = true;
    }
}
