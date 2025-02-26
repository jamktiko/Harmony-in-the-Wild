using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectButtonDefault : MonoBehaviour
{
    [FormerlySerializedAs("primaryButton")] [SerializeField] Button _primaryButton;
    // Start is called before the first frame update
    void Start()
    {
        _primaryButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
