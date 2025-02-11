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
    private bool canTriggerAudioChange = true;
    private bool initialTransformationPassed = false; // this bool is enabled a bit after the scene is loaded; it ensures the audio transformation will work when loading into the scene for the first time

    private void Start()
    {
        GameEventsManager.instance.uiEvents.OnUseUnstuckButton += DisableAudioChangeForAWhile;
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += DisableAudioChangeForCinematics;

        Invoke(nameof(EnableVFX), 2f);
    }

    private void OnDisable()
    {
        GameEventsManager.instance.uiEvents.OnUseUnstuckButton -= DisableAudioChangeForAWhile;
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= DisableAudioChangeForCinematics;
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
            Debug.Log("Change to arctic");
            if (canTriggerAudioChange)
            {
                StartCoroutine(StartArcticTheme());
            }

            redFox.SetActive(false);
            arcticFox.SetActive(true);

            currentAnimator = arcticFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = arcticFox.transform;
        }

        else
        {
            Debug.Log("Change to forest");
            if (canTriggerAudioChange)
            {
                StartCoroutine(StartForestTheme());
            }

            redFox.SetActive(true);
            arcticFox.SetActive(false);

            currentAnimator = redFox.GetComponent<Animator>();
            FoxMovement.instance.playerAnimator = currentAnimator;
            playerCamera.foxObject = redFox.transform;
        }
    }

    public void PrepareForModelChange(string modelName)
    {
        if (!initialTransformationPassed)
        {
            initialTransformationPassed = true;
            return;
        }

        if (canTriggerAudioChange)
        {
            AudioManager.instance.EndCurrentTheme();
        }

        if (modelName == "Arctic")
        {
            if (canTriggerVFX)
            {
                Debug.Log("Trigger model change with VFX.");
                ChangeVFX(true);
                StartCoroutine(MaintainRotation(5));
            }

            else
            {
                Debug.Log("Trigger model change without VFX.");
                StartCoroutine(ChangeModel(true, 0f));
            }
        }

        else if (modelName == "Forest")
        {
            if (canTriggerVFX)
            {
                Debug.Log("Trigger model change with VFX.");
                ChangeVFX(false);
                StartCoroutine(MaintainRotation(5));
            }

            else
            {
                Debug.Log("Trigger model change without VFX.");
                StartCoroutine(ChangeModel(false, 0f));
            }
        }
    }

    private void EnableVFX()
    {
        canTriggerVFX = true;
    }

    private void DisableAudioChangeForAWhile()
    {
        canTriggerAudioChange = false;

        Invoke(nameof(EnableAudioChange), 1f);
    }

    private void EnableAudioChange()
    {
        canTriggerAudioChange = true;
    }

    private void DisableAudioChangeForCinematics()
    {
        canTriggerAudioChange = false;
    }

    private IEnumerator StartArcticTheme()
    {
        Debug.Log("Waiting for arctic theme transition to be triggered...");

        yield return new WaitUntil(() => AudioManager.instance.themeTransitionOn == false && !AudioManager.instance.themeIsPlaying);

        Debug.Log("Arctic theme about to be triggered...");

        AudioManager.instance.StartNewTheme(ThemeName.Theme_Arctic);
    }

    private IEnumerator StartForestTheme()
    {
        Debug.Log("Waiting for forest theme transition to be triggered...");

        yield return new WaitUntil(() => AudioManager.instance.themeTransitionOn == false && !AudioManager.instance.themeIsPlaying);

        Debug.Log("Forest theme about to be triggered...");

        AudioManager.instance.StartNewTheme(ThemeName.Theme_Forest);
    }
}
