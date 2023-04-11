using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisonDetect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //score +1 majd
        Debug.Log("Objektum belépett a hitbox-ba!");
    }
}
