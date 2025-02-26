using System.Collections;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField] bool isDisclaimer;
    [SerializeField] bool isText;
    void Start()
    {
        if (isDisclaimer)
            StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<TMP_Text>()));
        else if (isText)
            StartCoroutine(DelayFadeTextToFullAlpha(1f, GetComponent<TMP_Text>()));
        else
            StartCoroutine(DelayMoreFadeTextToFullAlpha(1f, GetComponent<TMP_Text>()));
    }
    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    public IEnumerator DelayFadeTextToFullAlpha(float t, TMP_Text i)
    {
        yield return new WaitForSeconds(1);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    public IEnumerator DelayMoreFadeTextToFullAlpha(float t, TMP_Text i)
    {
        yield return new WaitForSeconds(2);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
