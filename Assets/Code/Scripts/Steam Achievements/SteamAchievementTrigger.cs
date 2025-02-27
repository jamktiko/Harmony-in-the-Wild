using UnityEngine;

public class SteamAchievementTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SteamManager.Instance.UnlockAchievement("FINISH_ACH");
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
