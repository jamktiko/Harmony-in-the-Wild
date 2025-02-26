using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class HitCounter : MonoBehaviour
{
    [FormerlySerializedAs("maxHits")]
    [Header("Config")]
    [Tooltip("Maximum amount of non-instakill hits before the player is transformed to the starting spot")]
    [SerializeField] private int _maxHits = 5;
    [FormerlySerializedAs("hitEvent")]
    [Tooltip("Possible hit event to take place every time player gets hit IF no death event is triggered")]
    [SerializeField] private UnityEvent _hitEvent;
    [FormerlySerializedAs("deathEvent")]
    [Tooltip("Possible event to take place when the player has got max hits or faces instakill")]
    [SerializeField] private UnityEvent _deathEvent;

    [FormerlySerializedAs("startingSpot")]
    [Header("Needed References")]
    [SerializeField] private Transform _startingSpot;
    [FormerlySerializedAs("visualIndicator")] [SerializeField] private HitCounterVisualIndicator _visualIndicator;

    private int _currentHits;

    // missing hit pop up !!
    // need to figure out how to implement it

    public void TakeHit(bool isInstaKill)
    {
        if (isInstaKill)
        {
            ReturnPlayerToStart();

            if (_deathEvent != null)
            {
                _deathEvent.Invoke();
            }
        }

        else
        {
            _currentHits++;

            if (_currentHits >= _maxHits)
            {
                ReturnPlayerToStart();

                if (_deathEvent != null)
                {
                    _deathEvent.Invoke();
                }
            }

            else if (_hitEvent != null)
            {
                _hitEvent.Invoke();
            }
        }

        _visualIndicator.UpdateHealth(_currentHits);
    }

    private void ReturnPlayerToStart()
    {
        // move player back to starting spot
        transform.position = _startingSpot.position;

        // reset current hits
        _currentHits = 0;

        _visualIndicator.UpdateHealth(_currentHits);
    }

    public int GetMaxHits()
    {
        return _maxHits;
    }
}
