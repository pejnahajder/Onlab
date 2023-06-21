using UnityEngine;

public class UpDownDrag : MonoBehaviour
{
    private GameObject basket;
    public GameObject preGamePanel;
    public GameObject inGamePanel;
    private NewPlayerBehaviour player;
    public void MoveUp()
    {
        //if(basket == null) basket = GameObject.FindGameObjectWithTag("basket");
        if (basket == null) basket = player.getBasket();
        
        basket.transform.position = new Vector3(basket.transform.position.x, basket.transform.position.y+0.1f, basket.transform.position.z);
    }

    public void MoveDown()
    {
        //if(basket == null) basket = GameObject.FindGameObjectWithTag("basket");
        if (basket == null) basket = player.getBasket();
        
        basket.transform.position = new Vector3(basket.transform.position.x, basket.transform.position.y-0.1f, basket.transform.position.z);
    }
    public void setPlayer(NewPlayerBehaviour pl)
    {
        player = pl;
    }
    
    public void DestroyScript()
    {
        Destroy(this);
        preGamePanel.SetActive(false);
        //inGamePanel.SetActive(true);
        player.startGame();
    }

    
}
