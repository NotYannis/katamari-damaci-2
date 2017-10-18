﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rigidbody;
    
    // The force value added to the rigidbody of the ball to move it faster or slower
    public int speed = 1;
    private Vector3 vectorForward;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X"));
    }

    // Move the ball by adding force to its rigidbody along the forward vector of the camera (always forward the player's view).
    public void moveForwardVector(Vector3 vectorForward)
    {
        rigidbody.AddForce(vectorForward * 100 * speed);
    }

}