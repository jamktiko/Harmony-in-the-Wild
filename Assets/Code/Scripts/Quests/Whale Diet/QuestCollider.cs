using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollider : MonoBehaviour
{
    [SerializeField] private bool isPreQuestCollider;
    [SerializeField] private bool isMidQuestCollider;

    private void Start()
    {
        /*if (isPreQuestCollider)
        {
            //transform.parent.GetComponent<PreQuestCollider>().
        }

        else if (isMidQuestCollider)
        {
            transform.parent.GetComponent<MidQuestCollider>().AddChildTransformToList(transform);
        }*/

        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.x = 0;
        newRotation.z = 0;

        transform.rotation = Quaternion.Euler(newRotation);
    }


}
