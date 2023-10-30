using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool used = false;
    BossDoorScript doorScript;
    [SerializeField] Material usedMat;
    private void Start()
    {
        doorScript = FindObjectOfType<BossDoorScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive&&!used)
        {
            used = true;
            Debug.Log("Lever pulled!");
            doorScript.usedlevers++;
            gameObject.GetComponent<MeshRenderer>().material= usedMat;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isActive = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }
}
