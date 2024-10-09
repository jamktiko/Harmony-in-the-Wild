using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    [SerializeField] PlayerModelToggle modelToggle;

    [Header("Forest")]
    [SerializeField] AudioSource forest;
    [SerializeField] AudioSource forestTheme;

    [Header("Arctic")]
    [SerializeField] AudioSource arctic;
    [SerializeField] AudioSource arcticTheme;

    private bool onForestSide = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            modelToggle.TogglePlayerModel();

            onForestSide = !onForestSide;

            if (onForestSide)
            {
                arcticTheme.Stop();
                forestTheme.Play();
            }

            else
            {
                forestTheme.Stop();
                arcticTheme.Play();
            }
        }
    }
}
