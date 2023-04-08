using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownDrag : MonoBehaviour
{
    private GameObject basket;
    public GameObject preGamePanel;
    public GameObject inGamePanel;
    public void MoveUp()
    {
        if(basket == null) basket = GameObject.FindGameObjectWithTag("basket");
        
        basket.transform.position = new Vector3(basket.transform.position.x, basket.transform.position.y+0.1f, basket.transform.position.z);
    }

    public void MoveDown()
    {
        if(basket == null) basket = GameObject.FindGameObjectWithTag("basket");
        
        basket.transform.position = new Vector3(basket.transform.position.x, basket.transform.position.y-0.1f, basket.transform.position.z);
    }
    
    public void DestroyScript()
    {
        Destroy(this);
        preGamePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
}
