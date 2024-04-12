using TMPro;
using UnityEngine;

public class AreYouSureMSG_Naming : MonoBehaviour
{
    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    void Update()
    {
        text.text = "Are you sure you want the name the fox: " + PlayerPrefs.GetString("foxName");
    }
}
