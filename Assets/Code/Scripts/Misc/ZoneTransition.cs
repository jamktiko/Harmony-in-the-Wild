using UnityEngine;
using UnityEngine.Events;

public class ZoneTransition : MonoBehaviour
{
    [Header("Forest")]
    [SerializeField] AudioSource forest;
    [SerializeField] AudioSource forestTheme;
    [SerializeField] private GameObject redFoxModel;

    [Header("Arctic")]
    [SerializeField] AudioSource arctic;
    [SerializeField] AudioSource arcticTheme;
    [SerializeField] private GameObject arcticFoxModel;

    [Header("Result")]
    [SerializeField] private UnityEvent onTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnterEvent != null)
        {
            onTriggerEnterEvent.Invoke();
        }

        else
        {
            Debug.Log("No trigger events defined for trigger enter!");
        }

        //entered forest
        /*if (FoxMovement.instance.gameObject != null && arcticFoxModel.activeInHierarchy)
        {
            modelToggle.ChangeModelToForest();
            arcticTheme.Stop();
            forestTheme.Play();
        }

        //entered arctic
        if (FoxMovement.instance.gameObject != null && redFoxModel.activeInHierarchy)
        {
            modelToggle.ChangeModelToArctic();
            forestTheme.Stop();
            arcticTheme.Play();
        }*/

        //Debug.Log($"{name} entered");
    }
}


