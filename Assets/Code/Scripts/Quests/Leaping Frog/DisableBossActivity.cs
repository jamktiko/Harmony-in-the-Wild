using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBossActivity : MonoBehaviour
{
    [SerializeField] private ShootingRotatingBoss boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            boss.DisableShooting();
        }
    }
}
