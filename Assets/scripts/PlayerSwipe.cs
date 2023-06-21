
using UnityEngine;
using Mirror;
using UnityEngine.XR;

public class PlayerSwipe : NetworkBehaviour
{
    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    [SerializeField]
    float throwForceInZ = 50f; // to control throw force in Z direction
    // Start is called before the first frame update

    private GameObject ball = null;
    private bool disabled = true;

    public void setBall(GameObject Ball)
    {
        ball = Ball;
    }

    public void Enable()
    {
        disabled = false;
    }

    void Start()
    {
        ball = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled)
        {
            if (isLocalPlayer)
            {
            
                if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {

                    // getting touch position and marking time when you touch the screen
                    touchTimeStart = Time.time;
                    startPos = Input.GetTouch (0).position;
                }

                // if you release your finger
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {

                    // marking time when you release it
                    touchTimeFinish = Time.time;
                

                    // calculate swipe time interval 
                    timeInterval = touchTimeFinish - touchTimeStart;
                    //Debug.Log("intervallum: "+timeInterval);
                    if (timeInterval > 1) return;
                    // getting release finger position
                    endPos = Input.GetTouch(0).position;

                    // calculating swipe direction in 2D space
                    direction = startPos - endPos;

                    // add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces

                    Vector3 force = new Vector3(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY,
                        throwForceInZ / timeInterval);
                    //var rotation = new Quaternion(0f,camera.transform.rotation.y,0f,camera.transform.rotation.w);
                    force = transform.rotation * force;
                    //rb.AddForce (- direction.x * throwForceInXandY, - direction.y * throwForceInXandY, throwForceInZ / timeInterval);

                    //Debug.Log(force);
                    //ballSpawned = false;
                    if (ball != null)
                    {
                        //Debug.Log("baall nem null");
                        CmdThrowBall(ball, force);
                    }
                    disabled = true;
                }

            
            }
        }
       
    }
    
    [Command(requiresAuthority = false)]
    void CmdThrowBall(GameObject ball_, Vector3 force)
    {
        //Debug.Log("server  eldobja lol: ");
        ball_.GetComponent<ballScript>().ThrowBall();
        ball_.GetComponent<Rigidbody>().isKinematic = false;
        ball_.GetComponent<Rigidbody>().AddForce(force);
    }
}
