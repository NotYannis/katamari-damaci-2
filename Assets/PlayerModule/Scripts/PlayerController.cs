using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplatterSystem.Isometric;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private Rigidbody rigidbody;
    private Vector3 maxSpeed;
    
    // The force value added to the rigidbody of the ball to move it faster or slower
    public int speed = 180;
    private Vector3 vectorForward;


	private float startTime;
	private Vector3 startScale=Vector3.zero;
	private float yOrigin, yOld;
	public float secondeInGame = 10f; 
	public SplatterUserCharacterController splatter;
	private Settings myscript;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        GameObject data = GameObject.Find("Datasettings");
        if (data != null)
        {
            myscript = data.GetComponent<Settings>();
            speed = (int)myscript.speedBall;
        }
		yOrigin = this.transform.position.y;
		splatter = GetComponent<SplatterUserCharacterController> ();
    }

    private void Update()
	{
		transform.RotateAround(transform.position, Camera.main.transform.right, rigidbody.velocity.magnitude);
		if(myscript.mode) Miniaturisation ();

    }

    // Move the ball by adding force to its rigidbody along the forward vector of the camera (always forward the player's view).
    public void moveForwardVector(Vector3 vectorForward, float acceleration)
    {
        rigidbody.AddForce(vectorForward * 100 * speed * acceleration);
    }

    private void LateUpdate() {
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, 15);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentsInChildren<EntitiesDetection>().Length != 0)
        {
            if (other.GetComponentsInChildren<EntitiesDetection>()[0] != null)
            {
                other.GetComponentsInChildren<EntitiesDetection>()[0].enabled = true;
                other.GetComponentsInChildren<Rigidbody>()[0].isKinematic = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentsInChildren<EntitiesDetection>().Length != 0)
        {
            if (other.GetComponentsInChildren<EntitiesDetection>()[0] != null)
            {
                other.GetComponentsInChildren<EntitiesDetection>()[0].enabled = false;
                other.GetComponentsInChildren<Rigidbody>()[0].isKinematic = true;
            }
        }
    }

	void Miniaturisation()
	{
		if(startScale==Vector3.zero) 
			startScale = this.transform.localScale;
		float scaleChangment = (secondeInGame - (Time.time - startTime))/secondeInGame;
		yOld = this.transform.position.y;
		this.transform.localScale = startScale * scaleChangment;
		this.transform.position = new Vector3 (this.transform.position.x, yOrigin * scaleChangment, this.transform.position.z);
		splatter.paintPositionOffset.y = splatter.paintPositionOffset.y + (yOld - this.transform.position.y);

		if (this.transform.localScale.x <= 0) {
			this.transform.localScale = startScale;
			//SceneManager.LoadScene ("MenuScene");
			Application.Quit();
		}
	}
}
