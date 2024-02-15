using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerPosition : MonoBehaviour
{
   
    [SerializeField] Transform fox;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.SetLocalPositionAndRotation(new Vector3(fox.position.x,0, fox.position.z),Quaternion.Euler(-90, 0, fox.eulerAngles.y));
    }
}
