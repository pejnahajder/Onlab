using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.Networking;
using UnityEngine.UI;

public struct newNotification : NetworkMessage
{
    public int value;
}

public struct ObjectInformation : NetworkMessage
{
    public int type; //0 == ball, 1 == basket
    public uint sender;
    public Vector3 position;
    public Quaternion rotation;
}

public struct ballThrowedMessage : NetworkMessage
{
    public Vector3 force;
    public uint sender; 
}

public class NewPlayerBehaviour : NetworkBehaviour
{
    public GameObject ballPrefab;
    public GameObject basketPrefab;
    public newSwipe swipeScript;

    public GameObject BasketPanel;
    public GameObject BallPanel;

    private GameObject camera;
    private bool spawnBall = false;
    private bool canSpawn = true;
    public Canvas canvas;
    public Text scoreText;
    private int score = 0;
    public Text enemyScoreText;
    private int enemyScore = 0;
    private uint ballnetid;
    
    private GameObject basket = null;
    
    private GameObject ballToThrow = null;
    private GameObject enemyBall = null;

    private Transform worldCoordinateSystem;

    private tapToPlaceBasket taptoplace_script;
    private UpDownDrag updown_scirpt;
     
    
    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
        score = 0;
        enemyScore = 0;
        swipeScript.setScript(this);
        
        worldCoordinateSystem = GameObject.Find("WorldCoordinateSystem").transform;
            
        taptoplace_script = camera.GetComponentInParent<tapToPlaceBasket>();
         taptoplace_script.setPlayer(this); 
        updown_scirpt = camera.GetComponentInParent<UpDownDrag>();
         updown_scirpt.setPlayer(this); 
        
         spawnBall = false;
         canSpawn = true;
         
        if (isLocalPlayer)
        {
            NetworkClient.RegisterHandler<newNotification>(OnNotification);
            NetworkClient.RegisterHandler<ObjectInformation>(OnObjectInformation);
            NetworkClient.RegisterHandler<ballThrowedMessage>(OnballThrowedMessage);
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

    private void OnballThrowedMessage(ballThrowedMessage msg)
    {
        if (msg.sender != netId)
        {
            if (enemyBall != null)
            {
                enemyBall.GetComponent<newBallScript>().ThrowBall();
                enemyBall.GetComponent<Rigidbody>().isKinematic = false;
                enemyBall.GetComponent<Rigidbody>().AddForce(msg.force);
            }
           
            enemyBall = null;
        }

    }
    private void OnObjectInformation(ObjectInformation msg)
    {
        if (msg.sender == netId)
            return;
        
        if (msg.type == 0)
        {
            var tra = transform;
            GameObject ball = Instantiate(ballPrefab, msg.position , msg.rotation);
            ball.transform.parent = worldCoordinateSystem;
            enemyBall = ball;
        }
        else if (msg.type == 1)
        {
            var tra = transform;
            GameObject basket_ = Instantiate(basketPrefab, msg.position , msg.rotation);
            basket_.transform.parent = worldCoordinateSystem;
        }
      
    }

    private void OnNotification(newNotification msg)
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
            
            
        }
    }
    

    public void SpawnBall()
    {
        if (!canSpawn) return;
        
        var tra = transform;
        GameObject ball = Instantiate(ballPrefab, tra.position + tra.forward * 0.62f - tra.up * 0.22f, Quaternion.identity);
        // GameObject ball = Instantiate(ballPrefab, transform.position ,  transform.rotation);
        ball.transform.parent = worldCoordinateSystem;
        ball.GetComponent<newBallScript>().ClientScript = this;
        swipeScript.Enable();
        ballToThrow = ball;
        CmdSendBallInformation(ball.transform.position, ball.transform.rotation, netId);
        canSpawn = false;
    }
    
    [Command(requiresAuthority = false)]
    public void CmdSendBallInformation(Vector3 pos, Quaternion rot, uint netid)
    {
        NetworkServer.SendToAll(new ObjectInformation(){sender = netid, type = 0,position = pos, rotation = rot});
    }



    [Command(requiresAuthority = false)]
    public void CmdDisableBasketSpawn()
    {
        NetworkServer.SendToAll(new newNotification{value = 0});
    }

    public void Scored()
    {
        CmdClientScored((int)netId);
    }
    
    [Command(requiresAuthority = false)]
    public void CmdClientScored(int myNetId)
    {
        NetworkServer.SendToAll(new newNotification{value = myNetId});
    }

    public void BasketButtonClicked()
    {
        CmdDisableBasketSpawn();
        taptoplace_script.enabled = true;
        taptoplace_script.StartScript();

    }

    
    [Command(requiresAuthority = false)]
    public void CmdSendBasketInformation(Vector3 pos, Quaternion rot, uint netid)
    {
        NetworkServer.SendToAll(new ObjectInformation(){sender = netid, type = 1,position = pos, rotation = rot});
    }
    
    public void startGame()
    {
        var pos = basket.transform.position;
        var rot = basket.transform.rotation;
        CmdSendBasketInformation(pos, rot, netId);
        CmdStartGame();
    }
    
    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        NetworkServer.SendToAll(new newNotification{value = 1});
    }
    
   
    public GameObject getBasket()
    {
        return basket;
    }
    
    public void throwBall( Vector3 force)
    {
        if (ballToThrow != null)
        {
            ballToThrow.GetComponent<newBallScript>().ThrowBall();
            ballToThrow.GetComponent<Rigidbody>().isKinematic = false;
            ballToThrow.GetComponent<Rigidbody>().AddForce(force);
            CmdSendBallThrowMessage(force, netId);
        }
        
        ballToThrow = null;
        canSpawn = true;
    }
    
    [Command(requiresAuthority = false)]
    public void CmdSendBallThrowMessage(Vector3 force_, uint netid)
    {
        NetworkServer.SendToAll(new ballThrowedMessage(){sender = netid, force = force_});
    }
}

