using TMPro;
using UnityEngine;

public class AreYouSureMsgNaming : MonoBehaviour
{
    private TMP_Text _text;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = "Are you sure you want the name the fox: " + PlayerPrefs.GetString("foxName") + "?";
    }
}
