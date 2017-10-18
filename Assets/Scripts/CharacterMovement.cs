using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public float speed = 10f;

    private float rh;
    private float rv;
    private Rigidbody _rb;

	void Start () {
        _rb = GetComponent<Rigidbody>();	
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.Z)) {
            rv = 1;
        }

        if (Input.GetKey(KeyCode.S)) {
            rv = -1;
        }

        if (Input.GetKey(KeyCode.Q)) {
            rh = -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            rh = 1;
        }

        _rb.velocity = new Vector3(rh * speed, 0, rv * speed);

        rh = rv = 0f;
    }
}
