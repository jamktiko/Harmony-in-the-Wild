using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hitText;
    [SerializeField] private Animator animator;

    private void Start()
    {
        Invoke(nameof(SubscribeToRaceEvents), 1f);
    }

    private void SubscribeToRaceEvents()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onShowHitPopUp += ShowPopUp;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onShowHitPopUp -= ShowPopUp;
    }

    private void ShowPopUp(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.ClosingWall:
                hitText.text = "You were crushed by a closing wall!";
                break;

            case HitType.FallingRocks:
                hitText.text = "Too many hits from falling rocks!";
                break;

            case HitType.Killzone:
                hitText.text = "You fell into the kill zone!";
                break;
        }

        animator.SetTrigger("showAnim");
    }
}
