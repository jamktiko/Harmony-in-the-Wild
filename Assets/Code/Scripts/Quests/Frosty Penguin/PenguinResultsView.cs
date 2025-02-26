using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PenguinResultsView : MonoBehaviour
{
    [FormerlySerializedAs("timerText")] [SerializeField] private TextMeshProUGUI _timerText;
    //[SerializeField] private TextMeshProUGUI trophyText;
    //[SerializeField] private Image trophyImage;
    [FormerlySerializedAs("timer")] [SerializeField] private PenguinTimer _timer;
    [FormerlySerializedAs("dungeonQuestDialogue")] [SerializeField] private DungeonQuestDialogue _dungeonQuestDialogue;

    //[Header("Trophy Config")]
    //[SerializeField] private float highestTrophy = 120f;
    //[SerializeField] private float mediumTrophy = 180f;
    //[SerializeField] private float lowestTrophy = 240f;
    //[SerializeField] private List<Sprite> trophyImages;

    [FormerlySerializedAs("storybookSectionIndex")]
    [Header("Storybook Config")]
    [SerializeField] private int _storybookSectionIndex;

    private void OnEnable()
    {
        GameEventsManager.instance.DialogueEvents.OnEndDialogue += ShowView;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.DialogueEvents.OnEndDialogue -= ShowView;
    }

    private void ShowView()
    {
        if (_dungeonQuestDialogue != null)
        {
            if (_dungeonQuestDialogue.FinalDialogueCompleted())
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                transform.GetChild(0).gameObject.SetActive(true);

                // set timer text
                _timerText.text = _timer.GetFinalTimeAsString();
            }
        }

        else
        {
            Debug.LogWarning("No Dungeon Quest Dialogue component assigned to Results View. Please check inspector!");
        }
    }

    //private string SetTrophyText()
    //{
    //    string trophyResults = "";

    //    float time = timer.GetFinalTimeAsFloat();

    //    if(time <= highestTrophy)
    //    {
    //        trophyResults = "GOLDEN TROPHY";
    //        trophyImage.sprite = trophyImages[0];
    //        trophyImage.enabled = true;
    //    }

    //    else if(time > highestTrophy && time <= mediumTrophy)
    //    {
    //        trophyResults = "SILVER TROPHY";
    //        trophyImage.sprite = trophyImages[1];
    //        trophyImage.enabled = true;
    //    }

    //    else if(time > mediumTrophy && time <= lowestTrophy)
    //    {
    //        trophyResults = "BRONZE TROPHY";
    //        trophyImage.sprite = trophyImages[2];
    //        trophyImage.enabled = true;
    //    }

    //    else
    //    {
    //        trophyResults = "no trophies";
    //        trophyImage.enabled = false;
    //    }

    //    return trophyResults;
    //}

    public void TryAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToOverworld()
    {
        StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, SceneManagerHelper.Scene.Overworld, true);
        GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
    }
}
