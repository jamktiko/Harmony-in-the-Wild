using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerModelToggle : MonoBehaviour
{
    [FormerlySerializedAs("redFox")] public GameObject RedFox;
    [FormerlySerializedAs("arcticFox")] [SerializeField] private GameObject _arcticFox;
    [FormerlySerializedAs("changeEffect")] [SerializeField] private GameObject _changeEffect;
    [FormerlySerializedAs("playerCamera")] [SerializeField] private CameraMovement _playerCamera;
    [FormerlySerializedAs("changeControl")] [SerializeField] private GameObject _changeControl;
    [FormerlySerializedAs("changePSSnow")] [SerializeField] private ParticleSystem[] _changePSSnow;
    [FormerlySerializedAs("changePSAutumn")] [SerializeField] private ParticleSystem[] _changePSAutumn;

    private Animator _currentAnimator;

    private bool _canTriggerVFX = false; // this bool is enabled a bit after the scene is loaded; the point is to prevent VFX being triggered when entering the scene
    private bool _canTriggerAudioChange = true;
    private bool _initialTransformationPassed = false; // this bool is enabled a bit after the scene is loaded; it ensures the audio transformation will work when loading into the scene for the first time

    private void Awake()
    {
        GameEventsManager.instance.UIEvents.OnUseUnstuckButton += DisableAudioChangeForAWhile;
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics += DisableAudioChangeForCinematics;

        Invoke(nameof(EnableVFX), 3f);
    }

    private void OnDisable()
    {
        GameEventsManager.instance.UIEvents.OnUseUnstuckButton -= DisableAudioChangeForAWhile;
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics -= DisableAudioChangeForCinematics;
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
        if (model == 0 && !RedFox.activeInHierarchy || model == 2)
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
            if (RedFox.activeInHierarchy)
                _changeControl.transform.rotation = RedFox.transform.rotation;
            else
                _changeControl.transform.rotation = _arcticFox.transform.rotation;
            yield return null;
        }
    }

    private void ChangeVFX(bool snow)
    {
        if (snow)
            for (int i = 0; i < _changePSSnow.Length; i++)
            {
                _changePSSnow[i].Play();
            }
        else
            for (int i = 0; i < _changePSAutumn.Length; i++)
            {
                _changePSAutumn[i].Play();
            }

        StartCoroutine(ChangeModel(snow, 3.3f));
    }

    private IEnumerator ChangeModel(bool toArcticFox, float timeBeforeChange)
    {
        yield return new WaitForSeconds(timeBeforeChange);

        if (toArcticFox)
        {
            if (_canTriggerAudioChange)
            {
                StartCoroutine(StartArcticTheme());
            }

            RedFox.SetActive(false);
            _arcticFox.SetActive(true);

            _currentAnimator = _arcticFox.GetComponent<Animator>();
            FoxMovement.Instance.PlayerAnimator = _currentAnimator;
            _playerCamera.FoxObject = _arcticFox.transform;
        }

        else
        {
            Debug.Log("Change to forest");
            if (_canTriggerAudioChange)
            {
                StartCoroutine(StartForestTheme());
            }

            RedFox.SetActive(true);
            _arcticFox.SetActive(false);

            _currentAnimator = RedFox.GetComponent<Animator>();
            FoxMovement.Instance.PlayerAnimator = _currentAnimator;
            _playerCamera.FoxObject = RedFox.transform;
        }
    }

    public void PrepareForModelChange(string modelName)
    {
        if (_canTriggerAudioChange)
        {
            AudioManager.Instance.EndCurrentTheme();
        }

        //if (!initialTransformationPassed)
        //{
        //    initialTransformationPassed = true;
        //    return;
        //}

        if (modelName == "Arctic")
        {
            if (_canTriggerVFX)
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
            if (_canTriggerVFX)
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
        _canTriggerVFX = true;
    }

    private void DisableAudioChangeForAWhile()
    {
        _canTriggerAudioChange = false;

        Invoke(nameof(EnableAudioChange), 1f);
    }

    private void EnableAudioChange()
    {
        _canTriggerAudioChange = true;
    }

    private void DisableAudioChangeForCinematics()
    {
        _canTriggerAudioChange = false;
    }

    private IEnumerator StartArcticTheme()
    {
        Debug.Log("Waiting for arctic theme transition to be triggered...");

        yield return new WaitUntil(() => AudioManager.Instance.ThemeTransitionOn == false && !AudioManager.Instance.ThemeIsPlaying);

        Debug.Log("Arctic theme about to be triggered...");

        AudioManager.Instance.StartNewTheme(ThemeName.ThemeArctic);
    }

    private IEnumerator StartForestTheme()
    {
        Debug.Log("Waiting for forest theme transition to be triggered...");

        yield return new WaitUntil(() => AudioManager.Instance.ThemeTransitionOn == false && !AudioManager.Instance.ThemeIsPlaying);

        Debug.Log("Forest theme about to be triggered...");

        AudioManager.Instance.StartNewTheme(ThemeName.ThemeForest);
    }
}
