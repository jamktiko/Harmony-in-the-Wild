using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class QuestCompletedUI : MonoBehaviour
{
    public static QuestCompletedUI Instance;

    [FormerlySerializedAs("showTime")] [SerializeField] private float _showTime = 4f;

    private VisualEffect _completedEffect;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Quest Completed UI in the scene!");
            Destroy(gameObject);
        }

        Instance = this;

        foreach (Transform t in transform)
        {
            if (t.GetComponent<VisualEffect>())
            {
                _completedEffect = t.GetComponent<VisualEffect>();
                break;
            }
        }
    }

    public void ShowUI(string questName)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = questName;
        _completedEffect.gameObject.SetActive(true);
        _completedEffect.Stop();
        _completedEffect.Play();

        StartCoroutine(DelayBeforeHiding());
    }

    private void HideUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator DelayBeforeHiding()
    {
        yield return new WaitForSeconds(_showTime);

        HideUI();
        Invoke(nameof(HideVFX), 7f);
    }

    private void HideVFX()
    {
        _completedEffect.gameObject.SetActive(false);
    }
}
