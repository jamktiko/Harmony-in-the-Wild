using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelToggle : MonoBehaviour
{
    [SerializeField] private GameObject redFox;
    [SerializeField] private GameObject arcticFox;
    [SerializeField] private GameObject changeEffect;

    private Vector3 effectPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            effectPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            Instantiate(changeEffect, effectPosition, Quaternion.identity);
            effectPosition = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            Instantiate(changeEffect, effectPosition, Quaternion.identity);
            effectPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
            Instantiate(changeEffect, effectPosition, Quaternion.identity);

            if (redFox.activeInHierarchy)
            {
                redFox.SetActive(false);
                arcticFox.SetActive(true);
            }

            else
            {
                redFox.SetActive(true);
                arcticFox.SetActive(false);
            }
        }
    }
}
