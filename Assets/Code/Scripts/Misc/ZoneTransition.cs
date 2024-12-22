using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    [SerializeField] PlayerModelToggle modelToggle;

    [Header("Forest")]
    [SerializeField] AudioSource forest;
    [SerializeField] AudioSource forestTheme;
    [SerializeField] private GameObject redFoxModel;

    [Header("Arctic")]
    [SerializeField] AudioSource arctic;
    [SerializeField] AudioSource arcticTheme;
    [SerializeField] private GameObject arcticFoxModel;

    private void OnTriggerEnter(Collider other)
    {
        //entered forest
        if (FoxMovement.instance.gameObject != null && arcticFoxModel.activeInHierarchy)
        {
            modelToggle.TogglePlayerModelPublic(1);
            arcticTheme.Stop();
            forestTheme.Play();
        }

        //entered arctic
        if (FoxMovement.instance.gameObject != null && redFoxModel.activeInHierarchy)
        {
            modelToggle.TogglePlayerModelPublic(2);
            forestTheme.Stop();
            arcticTheme.Play();
        }

        //Debug.Log($"{name} entered");
    }
}
