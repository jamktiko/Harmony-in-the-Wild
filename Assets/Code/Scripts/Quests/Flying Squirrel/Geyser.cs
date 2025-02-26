using UnityEngine;
using UnityEngine.Serialization;

public class Geyser : MonoBehaviour
{
    [FormerlySerializedAs("cooldown")] [SerializeField] private float _cooldown;
    [FormerlySerializedAs("timePassed")] [SerializeField] private float _timePassed;
    [FormerlySerializedAs("strength")] [SerializeField] private int _strength = 1700;
    [FormerlySerializedAs("isErupting")] [SerializeField] private bool _isErupting;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_cooldown == 0)
        {
            _cooldown = 400;
        }
    }

    void FixedUpdate()
    {
        _timePassed += Time.deltaTime; //NOTE: Changed 1f with Time.deltaTime to make it non-framerate dependant.

        if (_timePassed >= _cooldown && !_isErupting)
        {
            /*timePassed = 0;*/
            Debug.Log("ERUPTION");
            _rb.AddForce(new Vector3(0, _strength, 0), ForceMode.Force);
            _isErupting = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_isErupting)
        {
            _timePassed = 0;
        }
        _isErupting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("deadge");
        }
    }
}
