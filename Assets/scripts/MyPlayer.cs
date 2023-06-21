using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MyPlayer : NetworkBehaviour
{
    public GameObject ballPrefab;
    private GameObject camera;
    private bool canSpawn = false;
    public Canvas canvas;
    public Text score;
    public Text enemyscore;
    
   // public GameObject Player;
    
    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
        canSpawn = false;
        if (isLocalPlayer)
        {
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }
       // GameObject _UI = GameObject.FindWithTag("UI");
       // UI ui = (UI)_UI.GetComponent(typeof(UI));
        //ui.setPlayer(this);
    }

   

    private void Update()
    {

        if (isLocalPlayer)
        {
            transform.position = camera.transform.position;
            transform.rotation = camera.transform.rotation;
            if (canSpawn)
            {
                Debug.Log("fired");
                CmdSpawnBall();
                canSpawn = false;
            }
           
        }
       
    }

    public void SpawnButtonClicked()
    {
        Debug.Log("clicked");
        canSpawn = true;

    }

    // spawning ball on server
    [Command(requiresAuthority = false)]
    void CmdSpawnBall()
    {
        var cam = camera.transform;
        Vector3 pos = cam.position;
        GameObject ball = Instantiate(ballPrefab, pos + cam.forward * 0.62f - cam.up * 0.22f, Quaternion.identity);

        // GameObject ball = Instantiate(ballPrefab, transform.position ,  transform.rotation);
        NetworkServer.Spawn(ball);
    }
    
}

