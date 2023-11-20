using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedboost : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speedIncrease;

    public void IncreasePlayerSpeed(GameObject player)
    {
        player.GetComponent<FoxMovement>().moveSpeed += speedIncrease;
    }
}
