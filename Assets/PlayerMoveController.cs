using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMoveController : MonoBehaviour
{

	public float moveSpeed = 0.5f;
	public float jumpSpeed = 0.5f;

	CharacterController controller;
	Animator animator;
	Vector2 velocity = new Vector2 ();
	readonly Vector2 gravity = new Vector2 (0f, -9.81f);
	bool lastGrounded = false;
	bool grounded = false;
	bool jumping = false;

	void Awake ()
	{

		controller = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();

	}

	void Start ()
	{		
	}

	Vector2 MoveControls ()
	{

		Vector2 motion = new Vector2 ();

		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) {

			motion.x = -moveSpeed;

		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {

			motion.x = moveSpeed;

		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {

			if (lastGrounded == true) {
				jumping = true;
				motion.y = jumpSpeed;
			}

		} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {

			// TODO

		}

		return motion;

	}

	void FixedUpdate ()
	{

		lastGrounded = grounded;
		grounded = false;
		jumping = false;

		Vector2 motion = MoveControls ();

		Vector2 lastVelocity = velocity;

		Vector2 acceleration = new Vector2 ();
		acceleration += gravity;

		if (lastGrounded) {
			//velocity.x = motion.x;

			// TODO trying to change x velocity ... still quickly but not in 1 frame
			// yeah looks great! the 8 is... it will take 8 frames to apply the desired x velocity
			// which I could easy enough convert to some time?
			// this is just another way of applying acceleration though right?
			// I aught to be able to combine this into my acceleration term, though 
			// maybe this x frames ramp is easier to understand 
			if (motion.x == 0) {
				motion.x = 0 - velocity.x;
			}
			velocity.x += (motion.x / 8);
			velocity.x = Mathf.Clamp (velocity.x, -moveSpeed, moveSpeed);

			// TODO
			// y velocity from jumping
			velocity.y += motion.y;
		}

		velocity = velocity + (acceleration * Time.deltaTime);

		// eular integration
		Vector2 velocityStep = (lastVelocity + velocity) * (0.5f * Time.fixedDeltaTime);

		controller.Move (velocityStep);

		if ((controller.collisionFlags & CollisionFlags.Below) != 0) {

			grounded = true;
			velocity.y = 0;

		}

		animator.SetFloat ("moving", Mathf.Clamp (velocity.x, -1, 1));

		// FIXME
		// will need a falling animation prob right... or maybe anim just holds
		// yeah this needs some thought ... how the animator is setup etc
		// also might want to blend between idle/moving and jumping? 
		//animator.SetBool ("grounded", jumping);

		// === problems:
		// ? if the transition comes off of the blend tree then there is a big delay before the jump starts
		// feels awful, branching from 'any state' fixes it.. but is that ideal?
		// AAAH, uncheck has exit time fixes that
		// ? transition back to grounded happens before animation finishes... wtf?
		// = something wrong with that clip, the complete one works
		// = something to do with looping maybe, finishes anim when it can still loop
		// if i connect it with the air anim it works again
		// ? still unsure about animation actually moving model.. surely I want to move it myself?
		// 

		AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo (0);
		if (jumping == true) {
			if (!animator.IsInTransition (0) && animStateInfo.fullPathHash != Animator.StringToHash ("Base Layer.jumpUp")) {
				animator.SetTrigger ("jump");
			}
		}

	}

}
