using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] float timePassed;
    Rigidbody rb;
    [SerializeField] int strength=15;
    [SerializeField] bool erupting;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cooldown==0) 
            {
            cooldown = 400;
                }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed += 1f;
        if (timePassed>=cooldown&&!erupting)
        {
            /*timePassed = 0;*/
            Debug.Log("ERUPTION");
            rb.AddForce(new Vector3(0,strength,0),ForceMode.Force);
            erupting = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (erupting)
        {
            timePassed = 0;
        }
        erupting =false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("deadge");
        }
    }
}
