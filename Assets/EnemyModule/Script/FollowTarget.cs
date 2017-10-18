using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

	public GameObject target;

	// Update is called once per frame
	void Update () {
		if (!target)
			return;
		this.transform.LookAt (target.transform);
		//this.transform.position
	}
}
