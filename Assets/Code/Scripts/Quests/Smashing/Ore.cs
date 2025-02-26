using UnityEngine;

public class Ore : MonoBehaviour
{
    private bool _playerIsNear;

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _playerIsNear)
        {

            Invoke("DestroyObject", 0.5f);
            SmashingReturnOre.Instance.PickUpOre();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = false;
        }
    }

    private void DestroyObject()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
