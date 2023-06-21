using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncronScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        var wcs = GameObject.FindWithTag("COORD");
        wcs.transform.position = transform.position;
        wcs.transform.rotation = transform.rotation;
    }
    
    //ha elugral akkor frissites
    
}
