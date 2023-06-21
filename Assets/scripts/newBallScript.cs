using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newBallScript : MonoBehaviour
{
    private GameObject camera;

    private NewPlayerBehaviour clientScript;

    public NewPlayerBehaviour ClientScript
    {
        get => clientScript;
        set => clientScript = value;
    }
	
    private bool ballThrowed = false;
	
    public void ThrowBall () {
        // Destroy ball in 7 seconds
        Invoke(nameof(DestroySelf), 7f);
    }
    
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    
}
