using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Freeze : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float aoeRadius;
    [SerializeField] private float cooldownDuration;

    [SerializeField] private Image coloredCooldownIndicator;

    [Header("Audio")]
    [SerializeField] AudioSource freezeAudio;

    private bool onCooldown;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && PlayerManager.instance.abilityValues[7] && !onCooldown)
        {
            ActivateFreeze();
        }
    }

    private void ActivateFreeze()
    {
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, aoeRadius, LayerMask.GetMask("Freezables"));
        Debug.Log(foundObjects.Length + " freezables found.");

        if(foundObjects != null)
        {
            foreach(Collider newObject in foundObjects)
            {
                Freezable freezable = newObject.gameObject.GetComponent<Freezable>();

                if (freezable)
                {
                    freezable.Freeze();
                    freezeAudio.Play();
                }
            }
        }

        if(SceneManager.GetActiveScene().name == "Dungeon_Penguin")
        {
            StartCoroutine(FreezeCooldown());
        }
    }

    private IEnumerator FreezeCooldown()
    {
        onCooldown = true;

        float updateFillAmount = 1 / (cooldownDuration * 100);
        coloredCooldownIndicator.fillAmount = 0;
        
        while(coloredCooldownIndicator.fillAmount != 1)
        {
            coloredCooldownIndicator.fillAmount += updateFillAmount;

            yield return new WaitForSeconds(0.01f);
        }

        onCooldown = false;
    }
}
