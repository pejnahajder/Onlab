using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.UI;


public struct Notification : NetworkMessage
{
    public int value;
}

public class PlayerBehaviour : NetworkBehaviour
{
    public GameObject ballPrefab;
    public GameObject basketPrefab;
    public PlayerSwipe swipeScript;

    public GameObject BasketPanel;
    public GameObject BallPanel;

    private GameObject camera;
    private bool canSpawn = false;
    public Canvas canvas;
    public Text scoreText;
    private int score = 0;
    public Text enemyScoreText;
    private int enemyScore = 0;
    private uint ballnetid;
    private GameObject BBall = null;
    private GameObject basket = null;

    private Transform worldCoordinateSystem;

    private tapToPlaceBasket taptoplace_script;
    private UpDownDrag updown_scirpt;
     
    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
        score = 0;
        enemyScore = 0;
        
        worldCoordinateSystem = GameObject.Find("WorldCoordinateSystem").transform;
            
        taptoplace_script = camera.GetComponentInParent<tapToPlaceBasket>();
        //taptoplace_script.setPlayer(this); 
        //TODO @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        updown_scirpt = camera.GetComponentInParent<UpDownDrag>();
      //  updown_scirpt.setPlayer(this);  
      //TODO @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        
        canSpawn = false;
        if (isLocalPlayer)
        {
            NetworkClient.RegisterHandler<Notification>(OnNotification);
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }

        scoreText.text = score.ToString();
        enemyScoreText.text = enemyScore.ToString();
        
        // GameObject _UI = GameObject.FindWithTag("UI");
        // UI ui = (UI)_UI.GetComponent(typeof(UI));
        //ui.setPlayer(this);
    }

    private void OnNotification(Notification msg)
    {
        if (msg.value == 1)
        {
            BallPanel.SetActive(true);
            
        }
        else if (msg.value == 0)
        {
            BasketPanel.SetActive(false);
        }
        else if (msg.value == netId)
        {
            score += 1;
            scoreText.text = score.ToString();
        }
        else
        {
            enemyScore += 1;
            enemyScoreText.text = enemyScore.ToString();
        }
    }
    
    public void setBasket(GameObject baask)
    {
        basket = baask;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            transform.position = camera.transform.position;
            transform.rotation = camera.transform.rotation;
            
            if (canSpawn)
            {
                CmdSpawnBall();
                
                canSpawn = false;
            }
        }
    }

    public void SpawnButtonClicked()
    {
        canSpawn = true;
    }
    
    [Command(requiresAuthority = false)]
    public void CmdDisableBasketSpawn()
    {
        NetworkServer.SendToAll(new Notification{value = 0});
    }

    public void Scored()
    {
        CmdClientScored((int)netId);
    }
    
    [Command(requiresAuthority = false)]
    public void CmdClientScored(int myNetId)
    {
        NetworkServer.SendToAll(new Notification{value = myNetId});
    }

    public void BasketButtonClicked()
    {
        CmdDisableBasketSpawn();
        taptoplace_script.enabled = true;
        taptoplace_script.StartScript();
    }


    [Command(requiresAuthority = false)]
    void CmdSpawnBasket(Vector3 position, Quaternion rotation)
    {
        GameObject basket_ = Instantiate(basketPrefab, position, rotation);
        basket_.transform.parent = worldCoordinateSystem;
        NetworkServer.Spawn(basket_);
        //RpcBasketSpawned();
    }

    [ClientCallback]
    public void startGame()
    {
        var pos = basket.transform.position;
        var rot = basket.transform.rotation;
        CmdSpawnBasket(pos,rot);
        basket.SetActive(false);
        Destroy(basket);
        CmdStartGame();
    }
    
    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        NetworkServer.SendToAll(new Notification{value = 1});
    }
    
    // spawning ball on server
    [Command(requiresAuthority = false)]
    void CmdSpawnBall()
    {
        var tra = transform;
        GameObject ball = Instantiate(ballPrefab, tra.position + tra.forward * 0.62f - tra.up * 0.22f, Quaternion.identity);
        // GameObject ball = Instantiate(ballPrefab, transform.position ,  transform.rotation);
        ball.transform.parent = worldCoordinateSystem;
        
        ball.GetComponent<ballScript>().ClientScript = this;
        NetworkServer.Spawn(ball);
        setBaall(ball);
       // swipeScript.setBall(ball);
    }

    [ClientRpc]
    void setBaall(GameObject ball)
    {
        BBall = ball;
        swipeScript.setBall(ball);
        swipeScript.Enable();

    }
    public GameObject getBasket()
    {
        return basket;
    }
    
}

