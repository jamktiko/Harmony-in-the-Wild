using UnityEngine;
using UnityEngine.Serialization;

public class TeleportBear : MonoBehaviour
{
    [FormerlySerializedAs("spawnPosition")] [SerializeField] private Vector3 _spawnPosition;
    private Vector3 _rotation = new Vector3(0, -66, 0);

    [FormerlySerializedAs("bear")] [SerializeField] private Transform _bear;

    [FormerlySerializedAs("UICanvas")] [SerializeField] private GameObject _uiCanvas;
    [FormerlySerializedAs("QuestUICanvas")] [SerializeField] private GameObject _questUICanvas;
    private void Start()
    {
        _bear = GameObject.Find("TutorialBear(Clone)").transform;
        _uiCanvas = GameObject.Find("Minimap");
        _questUICanvas = GameObject.Find("QuestUI_Visuals");
    }
    public void TeleportBearToTree()
    {
        _bear = GameObject.Find("TutorialBear(Clone)").transform;
        if (_bear != null)
        {
            _bear.position = _spawnPosition;
            _bear.localRotation = Quaternion.Euler(_rotation);
        }
    }
    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    public void EnableDisableMovement()
    {
        if (PlayerInputHandler.Instance.MoveInput.enabled)
        {
            PlayerInputHandler.Instance.MoveInput.Disable();
        }
        else
        {
            PlayerInputHandler.Instance.MoveInput.Enable();
        }
    }
    public void HideUI()
    {
        _uiCanvas.SetActive(!_uiCanvas.activeInHierarchy);
        _questUICanvas.SetActive(!_questUICanvas.activeInHierarchy);
    }
}