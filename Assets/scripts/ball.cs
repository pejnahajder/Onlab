
using UnityEngine;
using Mirror;

public class ball : NetworkBehaviour
{
	private GameObject camera;

	private PlayerBehaviour clientScript;

	public PlayerBehaviour ClientScript
	{
		get => clientScript;
		set => clientScript = value;
	}

	Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
	float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

	[SerializeField]
	float throwForceInXandY = 1f; // to control throw force in X and Y directions

	[SerializeField]
	float throwForceInZ = 50f; // to control throw force in Z direction

	public GameObject ballObject;
	private Rigidbody rb;
	private bool ballThrowed = false;

	void Start()
	{
		camera = GameObject.FindWithTag("MainCamera");
		rb = ballObject.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		if (!ballThrowed)
		{
			// if you touch the screen
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {

				// getting touch position and marking time when you touch the screen
				touchTimeStart = Time.time;
				startPos = Input.GetTouch (0).position;
			}

			// if you release your finger
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {

				// marking time when you release it
				touchTimeFinish = Time.time;

				// calculate swipe time interval 
				timeInterval = touchTimeFinish - touchTimeStart;

				// getting release finger position
				endPos = Input.GetTouch (0).position;

				// calculating swipe direction in 2D space
				direction = startPos - endPos;

				// add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
				rb.isKinematic = false;
				Vector3 force = new Vector3(- direction.x * throwForceInXandY, - direction.y * throwForceInXandY, throwForceInZ / timeInterval);
				//var rotation = new Quaternion(0f,camera.transform.rotation.y,0f,camera.transform.rotation.w);
				force = camera.transform.rotation * force;
				//rb.AddForce (- direction.x * throwForceInXandY, - direction.y * throwForceInXandY, throwForceInZ / timeInterval);
				//rb.AddForce(force);
				CmdThrowBall(ballObject, force);
			
				// Destroy ball in 7 seconds
				Invoke(nameof(DestroySelf), 7f);
				
			
				//Destroying the script
				//ballThrowed = true;
				//this.enabled = false;
				//rb = null;

			}
		}

	}
	
	
	[Command(requiresAuthority = false)]
	void CmdThrowBall(GameObject ball_, Vector3 force)
	{
		Debug.Log("server  eldobja lol");
		ball_.GetComponent<Rigidbody>().isKinematic = false;
		ball_.GetComponent<Rigidbody>().AddForce(force);
	}
	
	[Server]
    void DestroySelf()
    {
     	NetworkServer.Destroy(gameObject);
    }
}