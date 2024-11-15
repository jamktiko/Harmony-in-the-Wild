using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    [SerializeField] PlayerModelToggle modelToggle;
    [SerializeField] GameObject redFox;
    [SerializeField] GameObject arcticFox;

    [Header("Forest")]
    [SerializeField] AudioSource forest;
    [SerializeField] AudioSource forestTheme;

    [Header("Arctic")]
    [SerializeField] AudioSource arctic;
    [SerializeField] AudioSource arcticTheme;

    private void OnTriggerEnter(Collider other)
    {
        //entered forest
        if (FoxMovement.instance.gameObject != null && arcticFox.activeInHierarchy)
        {
            modelToggle.ChangeModelToForest();
            arcticTheme.Stop();
            forestTheme.Play();
        }

        //entered arctic
        if (FoxMovement.instance.gameObject != null && redFox.activeInHierarchy)
        {
            modelToggle.ChangeModelToArctic();
            forestTheme.Stop();
            arcticTheme.Play();
        }

        //Debug.Log($"{name} entered");
    }
}
