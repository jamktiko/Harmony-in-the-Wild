using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public struct QuestInfo
{
    public Quest Quest;
    [FormerlySerializedAs("indicator")] public Transform Indicator;
}
public class QuestWaypoint : MonoBehaviour
{
    [FormerlySerializedAs("img")] public Image Img;
    [FormerlySerializedAs("target")] public GameObject Target;
    [FormerlySerializedAs("mainCamera")] public Camera MainCamera;
    [FormerlySerializedAs("questUI")] public QuestUI QuestUI;
    [FormerlySerializedAs("text")] public TMP_Text Text;
    [FormerlySerializedAs("questMarker")] [SerializeField] GameObject _questMarker;

    [FormerlySerializedAs("questSO")] [SerializeField] QuestScriptableObject _questSo;
    [FormerlySerializedAs("QuestInfos")] [SerializeField] List<QuestInfo> _questInfos;

    private void OnEnable()
    {
        var activeQuest = QuestMenuManager.TrackedQuest;
        if (activeQuest == null)
        {
            _questMarker.SetActive(false);
            return;
        }
        GetNewQuestWaypointPosition();
    }
    //private void Start()
    //{
    //     GetNewQuestWaypointPosition();
    //}
    private void Update()
    {
        float minX = Img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = Img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = MainCamera.WorldToScreenPoint(Target.transform.position);

        if (Vector3.Dot((Target.transform.position - transform.position), transform.forward) < 0)
        {
            //target is behind player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        Img.transform.position = pos;

        Text.text = Mathf.Round(Vector3.Distance(Target.transform.position, FoxMovement.Instance.FoxMiddle.position)).ToString() + "m";
    }

    public void GetNewQuestWaypointPosition()
    {
        _questMarker.SetActive(true);
        var activeQuest = QuestMenuManager.TrackedQuest;
        if (activeQuest.State == QuestState.InProgress)
        {
            Target = activeQuest.GetCurrentQuestStepPrefab();

            var questStep = Target.GetComponent<QuestStep>();
            if (questStep.PositionInScene != Vector3.zero && questStep.HasWaypoint)
            {
                Target.transform.position = questStep.PositionInScene;
                return;
            }
            _questMarker.SetActive(false);
            return;
        }
        Target = Instantiate(new GameObject("WaypointTarget"), activeQuest.DefaultPosition, Quaternion.identity);

    }
}