using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput_Naming : MonoBehaviour
{
    string input;
    [SerializeField]GameObject sureMessage;
    [SerializeField] GameObject Inappropriate;
    public void SaveName(string s) 
    {
        if (s != "" 
            && !s.Contains("nigger")
            && !s.Contains("nigga")
            && !s.Contains("fuck") 
            && !s.Contains("shit") 
            && !s.Contains("kys") 
            && !s.Contains("kill") 
            && !s.Contains("kill") 
            && !s.Contains("rape") 
            && !s.Contains("cunt") 
            && !s.Contains("bitch")
            && !s.Contains("kurva")
            && !s.Contains("dead"))
        {
            Inappropriate.SetActive(false);
            input = s;
            PlayerPrefs.SetString("foxName", input);
            Debug.Log(PlayerPrefs.GetString("foxName"));
            sureMessage.SetActive(true);
        }
        else
        {
            sureMessage.SetActive(false);
            Inappropriate.SetActive(true);
        }
    }
    public void NoButton() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void YesButton()
    {
        SceneManager.LoadScene("OverWorld");
    }
}
