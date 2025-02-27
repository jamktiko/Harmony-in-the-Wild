using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectButtonDefault : MonoBehaviour
{
    [FormerlySerializedAs("primaryButton")] [SerializeField]
    private Button _primaryButton;
    // Start is called before the first frame update
    private void Start()
    {
        _primaryButton.Select();
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
