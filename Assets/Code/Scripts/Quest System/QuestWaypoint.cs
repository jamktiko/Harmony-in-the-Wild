using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct QuestInfo
{
    public Quest quest;
    public Transform indicator;
}
public class QuestWaypoint : MonoBehaviour
{
    public Image img;
    public GameObject target;
    public Camera mainCamera;
    public QuestUI questUI;
    public TMP_Text text;

    [SerializeField] List<QuestInfo> QuestInfos;

    private void OnEnable()
    {
        target = GameObject.FindObjectsOfType<QuestPoint>().Where(x => x.questInfoForPoint.id == questUI.getCurrentQuestName()).First().gameObject;
    }
    private void Start()
    {
         target = GameObject.FindObjectsOfType<QuestPoint>().Where(x => x.questInfoForPoint.displayName == questUI.getCurrentQuestName()).First().gameObject;
    }
    private void Update()
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = mainCamera.WorldToScreenPoint(target.transform.position);

        if (Vector3.Dot((target.transform.position-transform.position),transform.forward)<0)
        {
            //target is behind player
            if (pos.x<Screen.width/2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x,minX,maxX);
        pos.y = Mathf.Clamp(pos.y,minY,maxY);

        img.transform.position = pos;

        text.text = Mathf.Round(Vector3.Distance(target.transform.position, FoxMovement.instance.foxMiddle.position)).ToString() + "m";
    }

    public void GetNewQuestWaypointPosition() 
    {
        target = GameObject.FindObjectsOfType<QuestPoint>().Where(x => x.questInfoForPoint.displayName == QuestMenuManager.trackedQuest.info.displayName).First().gameObject;
    }
}
