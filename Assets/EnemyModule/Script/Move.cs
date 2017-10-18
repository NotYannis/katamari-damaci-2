using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	// Update is called once per frame
	void Update () 
	{
		MoveC(GetPlayerMove ());
	}

	/// <summary>
	/// Effectue le déplacement du personnage en fonction des inputs du joueur.
	/// </summary>
	public void MoveC(Vector3 direction)
	{
		//Si la direction entrée est inverse à la direction courante
		/*if (direction.x * transform.right.x < 0)
		{
			//on retourne le personnage.
			transform.Rotate(0, 180, 0);
		}*/

		this.GetComponent<Rigidbody>().velocity = direction*10;


	}

	/// <summary>
	/// recuperer la direction des joueurs lorsqu'on appuie sur les touches vertical et horizontal
	/// </summary>
	/// <returns>Vecteur avec la direction</returns>
	public Vector3 GetPlayerMove()
	{
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		return new Vector3(horizontal, 0, vertical);
	}
}
