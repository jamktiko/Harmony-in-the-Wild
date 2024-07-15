using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnoffSwimmingColliders : MonoBehaviour
{
    [SerializeField] GameObject colliders;
        [SerializeField] bool isActive = false;
        [SerializeField] public bool used = false;
        // Update is called once per frame
        void Update()
        {
            if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive)
            {
                used = true;
                Destroy(colliders);
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
