using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public struct QuestInfo
{
    public string name;
    public Transform indicator;
}
public class QuestWaypoint : MonoBehaviour
{
    public Image img;
    public GameObject target;
    public Camera mainCamera;
    public QuestUI QuestUI;
    public TMP_Text text;

    private void OnEnable()
    {
        target = GameObject.FindObjectsOfType<QuestPoint>().Where(x => x.questInfoForPoint.id == QuestUI.getCurrentQuestName()).First().gameObject;
    }
    private void Start()
    {
         target = GameObject.FindObjectsOfType<QuestPoint>().Where(x => x.questInfoForPoint.displayName == QuestUI.getCurrentQuestName()).First().gameObject;
    }
    private void Update()
    {
        
        img.transform.position = mainCamera.WorldToScreenPoint(target.transform.position);
        text.text = Mathf.Round(Vector3.Distance(target.transform.position, FoxMovement.instance.foxMiddle.position)).ToString() + "m";
    }

    public void GetNewQuestWaypointPosition() 
    {
        target = GameObject.FindObjectsOfType<QuestPoint>().Where(x => x.questInfoForPoint.displayName == QuestUI.getCurrentQuestName()).First().gameObject;
    }
}
