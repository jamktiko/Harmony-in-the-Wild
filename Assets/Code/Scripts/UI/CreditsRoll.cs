using UnityEngine;

public class CreditsRoll : MonoBehaviour
{
    [SerializeField] GameObject credits;

    [SerializeField] Vector3 move = new Vector3(0, 5, 0);

    private RectTransform rectTransform;

    private void Start()
    {
        Debug.Log("IN CREDITS");
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        transform.Translate(move);

        if (transform.position.y > 9000)
        {
            rectTransform.anchoredPosition = new Vector2(0f, -1200f);
        }

        //else
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        //}
    }
}
