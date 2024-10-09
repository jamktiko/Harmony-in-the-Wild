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

    private void OnTriggerEnter(Collider other)
    {
        //entered forest
        if (FoxMovement.instance.gameObject != null && gameObject.name == "RedFoxFinal")
        {
            modelToggle.ChangeModelToForest();
            arcticTheme.Stop();
            forestTheme.Play();
        }

        //entered arctic
        if (FoxMovement.instance.gameObject != null && gameObject.name == "ArcticFoxFinal")
        {
            modelToggle.ChangeModelToArctic();
            forestTheme.Stop();
            arcticTheme.Play();
        }

        //Debug.Log($"{name} entered");
    }
}
