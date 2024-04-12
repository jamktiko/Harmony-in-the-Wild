using UnityEngine;
using UnityEngine.Events;

public class HitCounter : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Maximum amount of non-instakill hits before the player is transformed to the starting spot")]
    [SerializeField] private int maxHits = 5;
    [Tooltip("Possible hit event to take place every time player gets hit IF no death event is triggered")]
    [SerializeField] private UnityEvent hitEvent;
    [Tooltip("Possible event to take place when the player has got max hits or faces instakill")]
    [SerializeField] private UnityEvent deathEvent;

    [Header("Needed References")]
    [SerializeField] private Transform startingSpot;
    [SerializeField] private HitCounterVisualIndicator visualIndicator;
 
    private int currentHits;

    public void TakeHit(bool isInstaKill)
    {
        if (isInstaKill)
        {
            ReturnPlayerToStart();

            if(deathEvent != null)
            {
                deathEvent.Invoke();
            }
        }

        else
        {
            currentHits++;

            if (currentHits >= maxHits)
            {
                ReturnPlayerToStart();

                if(deathEvent != null)
                {
                    deathEvent.Invoke();
                }
            }

            else if(hitEvent != null)
            {
                hitEvent.Invoke();
            }
        }

        visualIndicator.UpdateHealth(currentHits);
    }

    private void ReturnPlayerToStart()
    {
        // move player back to starting spot
        transform.position = startingSpot.position;

        // reset current hits
        currentHits = 0;

        visualIndicator.UpdateHealth(currentHits);
    }

    public int GetMaxHits()
    {
        return maxHits;
    }
}
