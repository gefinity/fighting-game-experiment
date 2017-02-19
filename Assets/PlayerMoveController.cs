using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMoveController : MonoBehaviour
{

	public float moveSpeed = 0.5f;

	CharacterController controller;
	Vector2 velocity = new Vector2 ();
	readonly Vector2 gravity = new Vector2 (0f, -9.81f);
	bool lastGrounded = false;
	bool grounded = false;

	void Awake ()
	{

		controller = GetComponent<CharacterController> ();

	}

	void Start ()
	{		
	}

	Vector2 controls ()
	{

		Vector2 motion = new Vector2 ();

		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) {

			motion.x = -moveSpeed;

		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {

			motion.x = moveSpeed;

		}

		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.UpArrow)) {

			// TODO

		} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {

			// TODO

		}

		return motion;

	}

	void FixedUpdate ()
	{

		lastGrounded = grounded;
		grounded = false;

		Vector2 motion = controls ();

		Vector2 lastVelocity = velocity;

		if (lastGrounded) {
			velocity.x = motion.x;
		}

		velocity.y += motion.y;

		Vector2 acceleration = gravity;

		velocity = velocity + (acceleration * Time.deltaTime);

		// eular integration
		//Vector2 velocityStep = velocity * Time.fixedDeltaTime;
		Vector2 velocityStep = (lastVelocity + velocity) * (0.5f * Time.fixedDeltaTime);

		controller.Move (velocityStep);

		if ((controller.collisionFlags & CollisionFlags.Below) != 0) {

			grounded = true;

		}

	}

}
