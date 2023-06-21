
using UnityEngine;
using Mirror;
public class SpawnBall : NetworkBehaviour
{
    [SerializeField]
    GameObject ball;
    
    private GameObject cam;
    
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
    }
    [Command]
    public void Spawn()
    {
        //var camera = GameObject.FindWithTag("MainCamera").transform;
        var camera = cam.transform;
        Vector3 pos = camera.position;
        //pos.y -= 0.22f;
        GameObject projectile = Instantiate (ball, pos + camera.forward * 0.62f - camera.up * 0.22f, Quaternion.identity);
        NetworkServer.Spawn(projectile);
        RpcBall();
    }
    [ClientRpc]
    void RpcBall()
    {
        //??
    }
    
}
