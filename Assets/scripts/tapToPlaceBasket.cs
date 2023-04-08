using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class tapToPlaceBasket : MonoBehaviour
{
    public GameObject objectToSpawn; 
    
    public GameObject preGamePanel;
    public GameObject inGamePanel;
    //private List<GameObject> SpawnedObjects;
    private GameObject basket;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private GameObject camera;
    private bool onTouchHold = false;

    [SerializeField] 
    private Camera arCamera;
    
    
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    private void Awake()
    {
        preGamePanel.SetActive(false);
        inGamePanel.SetActive(false);
        _arRaycastManager = GetComponent<ARRaycastManager>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    bool TryToGetPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
    void Update()
    {
        
        if (!TryToGetPosition(out Vector2 touchPosition))
                      return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.tag.Equals("basket"))
                    {
                        onTouchHold = true;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }
        }
        
        
        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
              {
                  var hitPose = hits[0].pose;
                  var newPosition = new Vector3(hitPose.position.x, hitPose.position.y+0.3f, hitPose.position.z);
                  Vector3 rot = camera.transform.rotation.eulerAngles;
                  rot = new Vector3(0,rot.y+180,0);
                  var rotation = Quaternion.Euler(rot);
                  if (basket == null)
                  {
                      basket = Instantiate(objectToSpawn, newPosition, rotation);
                      preGamePanel.SetActive(true);
                  }
                  else
                  {
                      if (onTouchHold)
                      {
                          basket.transform.position = newPosition;
                          basket.transform.rotation = rotation;
                      }
                  }
              }
    }

    public void DestroyScript()
    {
        Destroy(this);
        preGamePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
}