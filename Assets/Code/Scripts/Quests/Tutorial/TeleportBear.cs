using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBear : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    private Vector3 rotation = new Vector3(0, -66, 0);

    [SerializeField]private Transform bear;

    [SerializeField]private GameObject UICanvas;
    [SerializeField]private GameObject QuestUICanvas;
    private void Start()
    {
        bear = GameObject.Find("TutorialBear(Clone)").transform;
        UICanvas = GameObject.Find("Minimap");
        QuestUICanvas = GameObject.Find("QuestUI_Visuals");
    }
    public void TeleportBearToTree() 
    {
        bear = GameObject.Find("TutorialBear(Clone)").transform;
        if (bear != null)
        {
            bear.position = spawnPosition;
            bear.localRotation = Quaternion.Euler(rotation);
        }
    }
    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    public void EnableDisableMovement() 
    {
        if (PlayerInputHandler.instance.MoveInput.enabled)
        {
            PlayerInputHandler.instance.MoveInput.Disable();
        }
        else
        {
            PlayerInputHandler.instance.MoveInput.Enable();
        }
    }
    public void HideUI() 
    {
            UICanvas.SetActive(!UICanvas.activeInHierarchy);
            QuestUICanvas.SetActive(!QuestUICanvas.activeInHierarchy);
    }
}