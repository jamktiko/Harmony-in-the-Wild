using UnityEngine;

public class Geyser : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private float timePassed;
    [SerializeField] private int strength = 1700;
    [SerializeField] private bool isErupting;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cooldown == 0)
        {
            cooldown = 400;
        }
    }

    void FixedUpdate()
    {
        timePassed += Time.deltaTime; //NOTE: Changed 1f with Time.deltaTime to make it non-framerate dependant.

        if (timePassed >= cooldown && !isErupting)
        {
            /*timePassed = 0;*/
            Debug.Log("ERUPTION");
            rb.AddForce(new Vector3(0, strength, 0), ForceMode.Force);
            isErupting = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isErupting)
        {
            timePassed = 0;
        }
        isErupting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("deadge");
        }
    }
}
