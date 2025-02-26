using UnityEngine;
using UnityEngine.Serialization;

public class MouseLookNoClip : MonoBehaviour
{
    [FormerlySerializedAs("rotation")] [SerializeField] Vector2 _rotation = Vector2.zero;
    [FormerlySerializedAs("speed")] public float Speed = 3;
    [FormerlySerializedAs("player")] [SerializeField] Transform _player;

    void Update()
    {
        _rotation.y += PlayerInputHandler.Instance.LookInput.ReadValue<Vector2>().x;
        _rotation.x += -PlayerInputHandler.Instance.LookInput.ReadValue<Vector2>().y;
    }
}
