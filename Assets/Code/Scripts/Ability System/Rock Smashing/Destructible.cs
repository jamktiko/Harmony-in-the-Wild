using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float _averageTimeBetweenSmashableEffects;
    [SerializeField] private bool _needsToBeFrozen;
    [SerializeField] private bool _isQuestRock;

    [Header("Needed References")]
    [SerializeField] private GameObject _destroyedVersion;
    [SerializeField] private GameObject _oreVersion;
    [SerializeField] private GameObject _smashableEffect;

    private bool _hasOre;
    private bool _destroyCurrentObject;
    private bool _smashingInProgress;

    private void Start()
    {
        StartCoroutine(DelayBetweenSmashableEffects());
    }

    private void OnEnable()
    {
        _smashingInProgress = false;
    }

    private void SpawnSmashableEffect()
    {
        Instantiate(_smashableEffect, transform.parent);
        StartCoroutine(DelayBetweenSmashableEffects());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && AbilityManager.Instance._abilityStatuses.TryGetValue(Abilities.RockDestroying, out bool isUnlocked) && isUnlocked && !_smashingInProgress)
        {
            if (_needsToBeFrozen)
            {
                if (gameObject.GetComponent<Freezable>().IsFrozen)
                {
                    AudioManager.Instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                    _smashingInProgress = true;
                    CreateDestroyedVersion();
                }

                else
                {
                    Debug.Log(gameObject.name + " needs to be frozen first.");
                }
            }

            else if (_isQuestRock)
            {
                AudioManager.Instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                _smashingInProgress = true;
                CheckForOre();
            }

            else
            {
                AudioManager.Instance.PlaySound(AudioName.Ability_RockSmashing, transform);
                _smashingInProgress = true;
                _destroyCurrentObject = true;
                CreateDestroyedVersion();
            }
        }
    }

    private void CreateDestroyedVersion()
    {
        if (_destroyCurrentObject)
        {
            if (_isQuestRock && _hasOre)
            {
                Instantiate(_oreVersion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            else
            {
                Instantiate(_destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        else
        {
            Instantiate(_destroyedVersion, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    public void IncludeOre()
    {
        _hasOre = true;
    }

    public void CheckForOre()
    {
        if (_hasOre)
        {
            AudioManager.Instance.PlaySound(AudioName.Ability_RockSmashing, transform);
            _destroyCurrentObject = true;
            CreateDestroyedVersion();

            SmashingAttemptCounter.instance.UpdateProgress(_hasOre);
        }

        else
        {
            AudioManager.Instance.PlaySound(AudioName.Ability_RockSmashing, transform);
            _destroyCurrentObject = true;
            CreateDestroyedVersion();

            SmashingAttemptCounter.instance.UpdateProgress(_hasOre);
        }
    }

    private IEnumerator DelayBetweenSmashableEffects()
    {
        yield return new WaitForSeconds(Random.Range(_averageTimeBetweenSmashableEffects - 2f, _averageTimeBetweenSmashableEffects + 2f));

        SpawnSmashableEffect();
    }
}
