using UnityEngine;

public class HideDevVisualsOnPlay : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
}
