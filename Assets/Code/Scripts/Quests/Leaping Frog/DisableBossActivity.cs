using UnityEngine;
using UnityEngine.Serialization;

public class DisableBossActivity : MonoBehaviour
{
    [FormerlySerializedAs("boss")] [SerializeField] private ShootingRotatingBoss _boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            _boss.DisableShooting();
        }
    }
}
