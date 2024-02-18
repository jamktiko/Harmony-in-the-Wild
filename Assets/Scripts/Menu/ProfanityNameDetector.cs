using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfanityNameDetector : MonoBehaviour
{
    //NOTE: Consider implementing something like https://github.com/stephenhaunts/ProfanityDetector instead of creating our own library?
    
    [SerializeField] GameObject sureMessage;
    [SerializeField] GameObject inappropriateMessage;

    private string foxNameInput;

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
            inappropriateMessage.SetActive(false);
            foxNameInput = s;
            PlayerPrefs.SetString("foxName", foxNameInput);
            Debug.Log(PlayerPrefs.GetString("foxName"));
            sureMessage.SetActive(true);
        }
        else
        {
            sureMessage.SetActive(false);
            inappropriateMessage.SetActive(true);
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
