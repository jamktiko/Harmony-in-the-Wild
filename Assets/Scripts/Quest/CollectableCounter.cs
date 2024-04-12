using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public int Counter = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
