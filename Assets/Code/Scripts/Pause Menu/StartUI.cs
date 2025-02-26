using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class StartUI : MonoBehaviour
{
    [FormerlySerializedAs("alwaysDisplayMouse")] public bool AlwaysDisplayMouse;
    [FormerlySerializedAs("pauseCanvas")] public GameObject PauseCanvas;
    [FormerlySerializedAs("optionsCanvas")] public GameObject OptionsCanvas;
    [FormerlySerializedAs("controlsCanvas")] public GameObject ControlsCanvas;
    [FormerlySerializedAs("audioCanvas")] public GameObject AudioCanvas;

    protected bool _mInPause;
    protected PlayableDirector[] _mDirectors;

    void Start()
    {
        if (!AlwaysDisplayMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        _mDirectors = FindObjectsOfType<PlayableDirector>();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
    }

    public void ExitPause()
    {
        _mInPause = true;
        SwitchPauseState();
    }

    public void RestartLevel()
    {
        _mInPause = true;
        SwitchPauseState();
    }

    void Update()
    {
        if (PlayerInputHandler.Instance.PauseInput != null && PlayerInputHandler.Instance.PauseInput.WasPressedThisFrame())
        {
            SwitchPauseState();
        }
    }

    protected void SwitchPauseState()
    {
        if (_mInPause && Time.timeScale > 0 || !_mInPause)
            return;

        if (!AlwaysDisplayMouse)
        {
            Cursor.lockState = _mInPause ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_mInPause;
        }

        for (int i = 0; i < _mDirectors.Length; i++)
        {
            if (_mDirectors[i].state == PlayState.Playing && !_mInPause)
            {
                _mDirectors[i].Pause();
            }
            else if (_mDirectors[i].state == PlayState.Paused && _mInPause)
            {
                _mDirectors[i].Resume();
            }
        }

        if (_mInPause)
            PlayerInputHandler.Instance.PlayerInputActionMap.Disable();
        else
            PlayerInputHandler.Instance.PlayerInputActionMap.Enable();

        Time.timeScale = _mInPause ? 1 : 0;

        if (PauseCanvas)
            PauseCanvas.SetActive(!_mInPause);

        if (OptionsCanvas)
            OptionsCanvas.SetActive(false);

        if (ControlsCanvas)
            ControlsCanvas.SetActive(false);

        if (AudioCanvas)
            AudioCanvas.SetActive(false);

        _mInPause = !_mInPause;
    }
}
