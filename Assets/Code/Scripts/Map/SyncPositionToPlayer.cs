using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SyncPositionToPlayer : MonoBehaviour
{
    [FormerlySerializedAs("playerImage")] public Image PlayerImage;
    [FormerlySerializedAs("size")] public float Size;
    [FormerlySerializedAs("material")] public Material Material;
    private Transform _player;
    private void Start()
    {
        _player = FoxMovement.Instance.FoxMiddle.transform;
    }
    private void Update()
    {
        //align the player icon to the player rotation; i do - and +90 because my game north is not on 0 degrees
        PlayerImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -_player.eulerAngles.y + 90f));
        //size is the value of the orthographic size of the camera you toke the screenshot with. *2 because the orthograpic size represents half the height of the screen.
        Material.SetVector("_PlayerPos", _player.position / (Size * 2f));
    }
}
