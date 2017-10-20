using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // The GameObject which contained the PlayerController script, to move the ball
    public GameObject player;
    private Vector3 offsetValue;
    public int speedRotationCamera = 10;

	void Start ()
    {
        offsetValue = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 4);
	}

    private void FixedUpdate()
    {
        // Move the player (the ball) along the forward vector of this camera
        Transform transformVectorForward = transform;

        // Rotate the Vector by -45 degres on the X axis to cancel the default rotation of the camera
        transformVectorForward.Rotate(-45, 0, 0);

        // Move the vector on the ground, to at the same level of the player (behind it)
        transformVectorForward.position = new Vector3(transformVectorForward.position.x, 2, transformVectorForward.position.z);

        Vector3 vectorForwardCamera = transformVectorForward.forward;
        player.GetComponent<PlayerController>().moveForwardVector(vectorForwardCamera);
    }

    private void LateUpdate () {
        offsetValue = Quaternion.AngleAxis((Input.GetAxis("Mouse X") * ((float)speedRotationCamera/10)), Vector3.up) * offsetValue;

        // Always keep the camera behind the player when rotation (from Mouse X input change) is made
        transform.position = player.transform.position + offsetValue;
        transform.LookAt(player.transform.position);
    }
}
