using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToPlayer : MonoBehaviour
{
    [SerializeField] Transform fox;
    // Update is called once per frame
    void FixedUpdate()
    {
       transform.eulerAngles=new Vector3(-90,0,fox.eulerAngles.y);
    }
}
