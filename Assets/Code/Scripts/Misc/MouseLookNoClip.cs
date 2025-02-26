using UnityEngine;

public class MouseLookNoClip : MonoBehaviour
{
    [SerializeField] Vector2 rotation = Vector2.zero;
    public float speed = 3;
    [SerializeField] Transform player;

    void Update()
    {
        rotation.y += PlayerInputHandler.instance.LookInput.ReadValue<Vector2>().x;
        rotation.x += -PlayerInputHandler.instance.LookInput.ReadValue<Vector2>().y;
    }
}
