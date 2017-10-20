using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rigidbody;
    private Vector3 maxSpeed;
    
    // The force value added to the rigidbody of the ball to move it faster or slower
    public int speed = 1;
    private Vector3 vectorForward;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Camera.main.transform.right, rigidbody.velocity.magnitude);
    }

    // Move the ball by adding force to its rigidbody along the forward vector of the camera (always forward the player's view).
    public void moveForwardVector(Vector3 vectorForward, float acceleration)
    {
        rigidbody.AddForce(vectorForward * 100 * speed * acceleration);
    }

    private void LateUpdate() {
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, 15);
    }
}
