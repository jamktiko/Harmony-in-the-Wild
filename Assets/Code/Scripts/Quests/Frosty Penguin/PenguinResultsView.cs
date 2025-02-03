using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PenguinResultsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    //[SerializeField] private TextMeshProUGUI trophyText;
    //[SerializeField] private Image trophyImage;
    [SerializeField] private PenguinTimer timer;
    [SerializeField] private DungeonQuestDialogue dungeonQuestDialogue;

    //[Header("Trophy Config")]
    //[SerializeField] private float highestTrophy = 120f;
    //[SerializeField] private float mediumTrophy = 180f;
    //[SerializeField] private float lowestTrophy = 240f;
    //[SerializeField] private List<Sprite> trophyImages;

    [Header("Storybook Config")]
    [SerializeField] private int storybookSectionIndex;

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += ShowView;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= ShowView;
    }

    private void ShowView()
    {
        if(dungeonQuestDialogue != null)
        {
            if (dungeonQuestDialogue.FinalDialogueCompleted())
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                transform.GetChild(0).gameObject.SetActive(true);

                // set timer text
                timerText.text = timer.GetFinalTimeAsString();
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
        StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, SceneManagerHelper.Scene.Overworld_VS, true);
        GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
    }
}
