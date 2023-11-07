using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("Config")]
    //[SerializeField] private float timeOnLoading;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float midDelay;
    [SerializeField] private float textUpdateSpeed;

    [Header("Needed References")]
    [SerializeField] private Image backgroundColor;
    [SerializeField] private Image foxImage;
    [SerializeField] private TextMeshProUGUI loadingText;

    private bool fading;
    private bool onLoading;
    private Color fadeState = new Color(0, 0, 0);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleLoadingScreenVisibility(true);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleLoadingScreenVisibility(false);
        }
    }

    public void ToggleLoadingScreenVisibility(bool show)
    {
        if (show)
        {
            StartCoroutine(ShowLoadingScreen());
        }

        else
        {
            onLoading = false;
        }
    }

    private IEnumerator ShowLoadingScreen()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        // show background
        fading = true;
        fadeState = backgroundColor.color;

        while (fading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a + 0.01f);
            backgroundColor.color = fadeState;

            if(backgroundColor.color.a >= 1)
            {
                fading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        // show fox image
        fading = true;
        fadeState = foxImage.color;

        while (fading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a + 0.01f);
            foxImage.color = fadeState;

            if (foxImage.color.a >= 1)
            {
                fading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        StartCoroutine(UpdateLoadingText());
    }

    private IEnumerator UpdateLoadingText()
    {
        loadingText.text = "Loading";

        fading = true;
        fadeState = loadingText.color;

        // show text
        while (fading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a + 0.01f);
            loadingText.color = fadeState;

            if (loadingText.color.a >= 1)
            {
                fading = false;
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        bool updateText = true;
        onLoading = true;

        // update text until loading screen starts to fade out
        while (updateText)
        {
            loadingText.text = "Loading";

            yield return new WaitForSeconds(textUpdateSpeed);

            loadingText.text = "Loading.";

            yield return new WaitForSeconds(textUpdateSpeed);

            loadingText.text = "Loading..";

            yield return new WaitForSeconds(textUpdateSpeed);

            loadingText.text = "Loading...";

            yield return new WaitForSeconds(textUpdateSpeed);

            if (!onLoading)
            {
                updateText = false;
            }
        }

        StartCoroutine(HideLoadingScreen());
    }

    private IEnumerator HideLoadingScreen()
    {
        fading = true;
        fadeState = loadingText.color;

        // hide text
        while (fading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a - 0.01f);
            loadingText.color = fadeState;

            if (loadingText.color.a <= 0)
            {
                fading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        // hide fox image
        fading = true;
        fadeState = foxImage.color;

        while (fading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a - 0.01f);
            foxImage.color = fadeState;

            if (foxImage.color.a <= 0)
            {
                fading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        // hide background
        fading = true;
        fadeState = backgroundColor.color;

        while (fading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a - 0.01f);
            backgroundColor.color = fadeState;

            if (backgroundColor.color.a <= 0)
            {
                fading = false;
            }

            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
