using ProfanityFilter;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ProfanityNameDetector : MonoBehaviour
{
    //NOTE: Consider implementing something like https://github.com/stephenhaunts/ProfanityDetector instead of creating our own library?

    [FormerlySerializedAs("sureMessage")] [SerializeField] GameObject _sureMessage;
    [FormerlySerializedAs("inappropriateMessage")] [SerializeField] GameObject _inappropriateMessage;

    private string _foxNameInput;

    public void SaveName(string s)
    {
        var filter = new ProfanityFilterScript();
        if (!filter.IsProfanity(s))
        {
            _inappropriateMessage.SetActive(false);
            _foxNameInput = s;
            PlayerPrefs.SetString("foxName", _foxNameInput);
            //Debug.Log(PlayerPrefs.GetString("foxName"));
            _sureMessage.SetActive(true);
        }
        else
        {
            _sureMessage.SetActive(false);
            _inappropriateMessage.SetActive(true);
        }
    }
    public void NoButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void YesButton()
    {
        SceneManagerHelper.LoadScene(SceneManagerHelper.Scene.Overworld);
    }
}
