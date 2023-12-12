using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsRoll : MonoBehaviour
{
    [SerializeField] GameObject credits;

    [SerializeField] Vector3 move = new Vector3(0, 5, 0);

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        transform.Translate(move);

        if (transform.position.y > 9000)
        {
            rectTransform.anchoredPosition = new Vector2(0f, -1200f);
        }
    }
}
