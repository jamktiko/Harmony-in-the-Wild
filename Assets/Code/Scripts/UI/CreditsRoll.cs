using UnityEngine;
using UnityEngine.Serialization;

public class CreditsRoll : MonoBehaviour
{
    [FormerlySerializedAs("credits")] [SerializeField]
    private GameObject _credits;

    [FormerlySerializedAs("move")] [SerializeField]
    private Vector3 _move = new Vector3(0, 5, 0);

    private RectTransform _rectTransform;

    private void Start()
    {
        Debug.Log("IN CREDITS");
        _rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        transform.Translate(_move);

        if (transform.position.y > 9000)
        {
            _rectTransform.anchoredPosition = new Vector2(0f, -1200f);
        }

        //else
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        //}
    }
}
