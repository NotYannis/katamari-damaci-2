using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesDetection : MonoBehaviour {

	//public bool attack = false;
	private GameObject perso;
	public int speed = 1;
	public int detectionDistance = 20;
	public bool jump = false;
	private bool isJumping = false;

	// Use this for initialization
	void Start () 
	{
		PlayerController[] persoTmp = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
		perso = persoTmp[0].gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float x, z;
		if (Mathf.Abs (this.transform.position.x - perso.transform.position.x) + Mathf.Abs (this.transform.position.z - perso.transform.position.z) > detectionDistance)
			return;
		if (this.transform.position.x - perso.transform.position.x > -10 && this.transform.position.x - perso.transform.position.x <= 0) {
			x = -0.01f * (10 - Mathf.Abs (this.transform.position.x - perso.transform.position.x));
		} else if (this.transform.position.x - perso.transform.position.x < 10 && this.transform.position.x - perso.transform.position.x >= 0) {
			x = 0.01f * (10 - Mathf.Abs (this.transform.position.x - perso.transform.position.x));
		} else {
			x = 0;
		}
		x *= speed;
		if (this.transform.position.z - perso.transform.position.z > -10 && this.transform.position.z - perso.transform.position.z <= 0) {
			z = -0.01f * (10 - Mathf.Abs (this.transform.position.z - perso.transform.position.z));
		} else if (this.transform.position.z - perso.transform.position.z < 10 && this.transform.position.z - perso.transform.position.z >= 0) {
			z = 0.01f * (10 - Mathf.Abs (this.transform.position.z - perso.transform.position.z));
		} else {
			z = 0;
		}
		z *= speed;

		this.transform.position = new Vector3 (this.transform.position.x+x, this.transform.position.y, this.transform.position.z+ z);

		if((x!=0 || z!=0) && !isJumping && jump)
		{
			GetComponent<Rigidbody>().AddForce(new Vector3(0, 1000, 0), ForceMode.Impulse);
			isJumping = true;
		}
	}

	void OnCollisionEnter (Collision hit)
	{
		isJumping = false;
	}
}
