using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    [SerializeField]
    GameObject ball;
    
    private GameObject cam;

    private bool canSpawn;
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        InvokeRepeating("CanSpawn", 0, 5f);

    }
    public void CanSpawn()
    {
        canSpawn = true;
    }
    public void Spawn()
    {
        //var camera = GameObject.FindWithTag("MainCamera").transform;
        var camera = cam.transform;
        Vector3 pos = camera.position;
        //pos.y -= 0.22f;
        Instantiate (ball, pos + camera.forward * 0.62f - camera.up * 0.22f, Quaternion.identity);
        canSpawn = false;
    }
    
}
