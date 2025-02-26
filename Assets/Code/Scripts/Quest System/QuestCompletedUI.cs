using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class QuestCompletedUI : MonoBehaviour
{
    public static QuestCompletedUI instance;

    [SerializeField] private float showTime = 4f;

    private VisualEffect completedEffect;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Quest Completed UI in the scene!");
            Destroy(gameObject);
        }

        instance = this;

        foreach (Transform t in transform)
        {
            if (t.GetComponent<VisualEffect>()) 
            { 
                completedEffect = t.GetComponent<VisualEffect>();
                break;
            }
        }
    }

    public void ShowUI(string questName)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = questName;
        completedEffect.gameObject.SetActive(true);
        completedEffect.Stop();
        completedEffect.Play();

        StartCoroutine(DelayBeforeHiding());
    }

    private void HideUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator DelayBeforeHiding()
    {
        yield return new WaitForSeconds(showTime);

        HideUI();
        Invoke(nameof(HideVFX), 7f);
    }

    private void HideVFX()
    {
        completedEffect.gameObject.SetActive(false);
    }
}
