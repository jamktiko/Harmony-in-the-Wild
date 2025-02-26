using UnityEngine;

public class LBCrowProximity : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "lb_bird")
        {
            col.SendMessage("CrowIsClose");
        }
    }

}
