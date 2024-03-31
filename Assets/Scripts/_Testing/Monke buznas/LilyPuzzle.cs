using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LilyPuzzle : MonoBehaviour
{
    public static LilyPuzzle instance;
    [SerializeField] private Transform[] lilyTransforms;

    [HideInInspector]public int socketsFilled = Mathf.Clamp(0, 0, 3);

    private Vector3[] lilyInitialPositions;
    private int correctLilies = Mathf.Clamp(0, 0, 3);
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one LilyPuzzle object.");
            Destroy(gameObject);
        }
        instance = this;
    }
    void Start()
    {
        //store lily positions
        lilyInitialPositions = new Vector3[lilyTransforms.Length];
        for (int i = 0; i < lilyTransforms.Length; i++)
        {
            lilyInitialPositions[i] = lilyTransforms[i].position;
        }
    }
    public void CheckPuzzleProgress(int change)
    {
        correctLilies += change;

        if (socketsFilled >= 3)
        {
            if (correctLilies < 3)
            {
                //puzzle failed, reset it
                Invoke("ResetPuzzle", 1f);
            }
            else
            {
                //complete puzzle, open door
                BossDoorMonkey.instance.CompletePuzzle();
            }
        }

        Debug.Log("Progress is: " + correctLilies);
    }
    void ResetPuzzle()
    {
        for (int i = 0; i < lilyTransforms.Length; i++)
        {
            lilyTransforms[i].position = lilyInitialPositions[i];
        }

        //whatever else. particles or audio idk
    }
}
