using System.Collections;
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

    private bool isFading;
    private bool isLoading;
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
            isLoading = false;
        }
    }

    private IEnumerator ShowLoadingScreen()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        // show background
        isFading = true;
        fadeState = backgroundColor.color;

        while (isFading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a + 0.01f);
            backgroundColor.color = fadeState;

            if(backgroundColor.color.a >= 1)
            {
                isFading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        // show fox image
        isFading = true;
        fadeState = foxImage.color;

        while (isFading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a + 0.01f);
            foxImage.color = fadeState;

            if (foxImage.color.a >= 1)
            {
                isFading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        StartCoroutine(UpdateLoadingText());
    }

    private IEnumerator UpdateLoadingText()
    {
        loadingText.text = "Loading";

        isFading = true;
        fadeState = loadingText.color;

        // show text
        while (isFading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a + 0.01f);
            loadingText.color = fadeState;

            if (loadingText.color.a >= 1)
            {
                isFading = false;
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        bool updateText = true;
        isLoading = true;

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

            if (!isLoading)
            {
                updateText = false;
            }
        }

        StartCoroutine(HideLoadingScreen());
    }

    private IEnumerator HideLoadingScreen()
    {
        isFading = true;
        fadeState = loadingText.color;

        // hide text
        while (isFading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a - 0.01f);
            loadingText.color = fadeState;

            if (loadingText.color.a <= 0)
            {
                isFading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        // hide fox image
        isFading = true;
        fadeState = foxImage.color;

        while (isFading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a - 0.01f);
            foxImage.color = fadeState;

            if (foxImage.color.a <= 0)
            {
                isFading = false;
                yield return new WaitForSeconds(midDelay);
            }

            yield return new WaitForSeconds(fadeSpeed);
        }

        // hide background
        isFading = true;
        fadeState = backgroundColor.color;

        while (isFading)
        {
            fadeState = new Color(fadeState.r, fadeState.g, fadeState.b, fadeState.a - 0.01f);
            backgroundColor.color = fadeState;

            if (backgroundColor.color.a <= 0)
            {
                isFading = false;
            }

            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
