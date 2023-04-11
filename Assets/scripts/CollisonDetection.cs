using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class CollisonDetection : MonoBehaviour
{
    public GameObject ball;
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("center"))
        {
            //pontot kap a jatekos mondjuk
            GameObject.Destroy(ball);
        }
        if (collision.collider.tag.Equals("ball"))
        {
            //pontot kap a jatekos mondjuk
            Debug.Log("asd");
        }

    }
    
}
