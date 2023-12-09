using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCompletedUI : MonoBehaviour
{
    public static QuestCompletedUI instance;

    [SerializeField] private float showTime = 4;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Quest Completed UI in the scene!");
            Destroy(gameObject);
        }

        instance = this;
    }

    public void ShowUI(string questName)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = questName;

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
    }
}
