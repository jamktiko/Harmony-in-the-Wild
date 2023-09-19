using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool used = false;
    [SerializeField] CollectableCounter collectableCounter;
    // Start is called before the first frame update
    void Start()
    {
        collectableCounter = GameObject.Find("CollectableCounter").GetComponent<CollectableCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&isActive)
        {
            used = true;
            collectableCounter.Counter++;
            Destroy(gameObject);
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
