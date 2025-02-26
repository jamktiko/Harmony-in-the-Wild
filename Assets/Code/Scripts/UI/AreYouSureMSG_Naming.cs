using TMPro;
using UnityEngine;

public class AreYouSureMsgNaming : MonoBehaviour
{
    TMP_Text _text;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }
    void Update()
    {
        _text.text = "Are you sure you want the name the fox: " + PlayerPrefs.GetString("foxName") + "?";
    }
}
