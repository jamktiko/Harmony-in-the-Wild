using UnityEngine;
using UnityEngine.Serialization;

public class DevShortCuts : MonoBehaviour
{
    [FormerlySerializedAs("player")]
    [Header("Needed References")]
    [SerializeField] private Transform _player;

    [FormerlySerializedAs("bossScene")]
    [Header("Shortcut Config")]
    [Tooltip("Press B & S to go to this boss scene")]
    [SerializeField] private string _bossScene;
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;

    [FormerlySerializedAs("newPosition")]
    [Tooltip("Press N & P to go to this position")]
    [SerializeField] private Transform _newPosition;

    private string _questId;
    private void Start()
    {
        if (_questSo != null)
        {
            _questId = _questSo.id;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.R))
        {
            //SceneManager.LoadScene("Overworld");
        }
        if (Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.S) && _questSo != null)
        {
            //GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId);
            //SceneManager.LoadScene(bossScene);
        }

        if (Input.GetKey(KeyCode.N) && Input.GetKeyDown(KeyCode.P))
        {
            //player.GetComponent<FoxMove>().enabled = false;
            //player.GetComponent<CharacterController>().enabled = false;

            _player.position = _newPosition.position;

            //player.GetComponent<FoxMove>().enabled = true;
            //player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
