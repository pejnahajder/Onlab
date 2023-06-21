
using UnityEngine;
using Mirror;

public class ballScript : NetworkBehaviour
{
	private GameObject camera;

	private PlayerBehaviour clientScript;

	public PlayerBehaviour ClientScript
	{
		get => clientScript;
		set => clientScript = value;
	}
	
	private bool ballThrowed = false;
	
	public void ThrowBall () {
		// Destroy ball in 7 seconds
		Invoke(nameof(DestroySelf), 7f);
	}
	
	
	[Server]
    void DestroySelf()
    {
     	NetworkServer.Destroy(gameObject);
    }
}