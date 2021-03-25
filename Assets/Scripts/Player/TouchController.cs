using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
	// public static TouchController Instance { get; private set; }

	// VARIABLES
	public CharacterController2D characterController2D;
	public Animator animator;
	public GameObject joystickGameobject;
	public PlayerStats playerStats;

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2.0f;

	Rigidbody2D rigidBody;
	Joystick joystick;

	float horizontalMove = 0f;

	bool isJumping = false;
	bool touchingGround = false;

	void Awake()
	{
		//if (Instance == null)
		//{
		//	Instance = this;
		//	DontDestroyOnLoad(gameObject);
		//}
		//else
		//{
		//	Destroy(gameObject);
		//}


		joystick = joystickGameobject.GetComponent<Joystick>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (joystick.Horizontal >= 0.2f)
		{
			horizontalMove = playerStats.Speed;
		}
		if (joystick.Horizontal <= -0.2f)
		{
			horizontalMove = -playerStats.Speed;
		}
		if (joystick.Horizontal > -0.2f && joystick.Horizontal < 0.2f)
		{
			horizontalMove = 0f;
		}

		if (joystick.Vertical >= 0.5f)
		{
			isJumping = true;
			animator.SetBool("IsJumping", true);
		}

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
		animator.SetBool("IsFalling", false);
	}


	private void FixedUpdate()
	{
		characterController2D.Move(horizontalMove * Time.fixedDeltaTime, playerStats.JumpPower, isJumping, out touchingGround);

		if (rigidBody.velocity.y < -0.25)
		{
			rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

			if (!touchingGround)
				animator.SetBool("IsFalling", true);
			else
				animator.SetBool("IsFalling", false);
		}
		else if (rigidBody.velocity.y >= -0.01)
		{
			animator.SetBool("IsFalling", false);

			if (!isJumping)
			{
				rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
			}
		}

		isJumping = false;
	}
}
