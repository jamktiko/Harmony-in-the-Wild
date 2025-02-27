using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public struct QuestInfo
{
    public Quest Quest;
    public Transform Indicator;
}
public class QuestWaypoint : MonoBehaviour
{
    public Image Img;
    public GameObject Target;
    public Camera MainCamera;
    public QuestUI QuestUI;
    public TMP_Text DistanceText;
    
    [SerializeField] private GameObject _questMarker;
    [SerializeField] private QuestScriptableObject _questSo;
    [SerializeField] private List<QuestInfo> _questInfos;

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
        UpdateWaypointPosition();
    }

    public void GetNewQuestWaypointPosition(bool active = true)
    {
        switch (active)
        {
            case true:
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
                return;
            
            case false:
                _questMarker.SetActive(false);
                return;
        }
        

    }

    private void UpdateWaypointPosition()
    {
        if (_questMarker.activeSelf)
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

            DistanceText.text =
                Mathf.Round(Vector3.Distance(Target.transform.position, FoxMovement.Instance.FoxMiddle.position))
                    .ToString() + "m";
        }
    }
}