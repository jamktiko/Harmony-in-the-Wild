using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBear : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    private Vector3 rotation = new Vector3(0, -66, 0);

    [SerializeField]private Transform bear;

    private void Start()
    {
        bear = GameObject.Find("TutorialBear(Clone)").transform;
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
}