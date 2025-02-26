using UnityEngine;
using UnityEngine.Serialization;

public class BossDoorMonkey : MonoBehaviour
{
    public static BossDoorMonkey Instance;

    [FormerlySerializedAs("anim")] [SerializeField] private Animator _anim;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one BossDoorMonkey object.");
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void CompletePuzzle()
    {
        gameObject.GetComponent<AudioSource>().Play();
        _anim.Play("Door_Open_ANI");
    }
}
