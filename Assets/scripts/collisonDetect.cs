
using System;
using Mirror;
using UnityEngine;

public class collisonDetect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        try
        {
            var ballscript = other.GetComponentInParent<ballScript>();
            if(ballscript!=null) ballscript.ClientScript.Scored();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}
