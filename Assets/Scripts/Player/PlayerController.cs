using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	PlayerStats playerStats;
	[SerializeField]
	public Joystick joystick;
	[SerializeField]
	private float groundCheckRadius;
	[SerializeField]
	private float slopeCheckDistance;
	[SerializeField]
	private float maxSlopeAngle;
	[SerializeField]
	private Transform groundCheck;
	[SerializeField]
	private LayerMask whatIsGround;
	[SerializeField]
	private PhysicsMaterial2D noFriction;
	[SerializeField]
	private PhysicsMaterial2D fullFriction;
	[SerializeField]
	private Animator animator;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField]
	private float fallMultiplier;
	[SerializeField]
	private float lowJumpMultiplier;

	private float horizontalMove;

	private float xInput;
	private float slopeDownAngle;
	private float slopeSideAngle;
	private float lastSlopeAngle;

	private int facingDirection = 1;

	private bool isGrounded;
	private bool isOnSlope;
	private bool isJumping;
	private bool canWalkOnSlope;
	private bool canJump;

	private Vector2 newVelocity;
	private Vector2 newForce;
	private Vector2 capsuleColliderSize;

	private Vector2 slopeNormalPerp;

	private Vector3 m_Velocity = Vector3.zero;

	private Rigidbody2D rb;
	private CapsuleCollider2D cc;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		cc = GetComponent<CapsuleCollider2D>();

		capsuleColliderSize = cc.size;
	}

	private void Update()
	{
		CheckInput();
	}

	private void FixedUpdate()
	{
		CheckGround();
		SlopeCheck();
		ApplyMovement();
		CheckJumping();
	}

	private void CheckJumping()
	{
		if (rb.velocity.y < -0.25)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

			if (!isGrounded)
				animator.SetBool("IsFalling", true);
			else
				animator.SetBool("IsFalling", false);
		}
		else if (rb.velocity.y >= -0.01)
		{
			animator.SetBool("IsFalling", false);

			if (!isJumping)
			{
				rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
			}
		}
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
		animator.SetBool("IsFalling", false);
	}

	private void CheckInput()
	{
		xInput = joystick.Horizontal;

		if (xInput > 0.2f)
		{
			horizontalMove = playerStats.Speed;
			if (facingDirection == -1)
				Flip();
		}
		else if (xInput < -0.2f)
		{
			horizontalMove = -playerStats.Speed;
			if (facingDirection == 1)
				Flip();
		}

		if (xInput > -0.2f && xInput < 0.2f)
		{
			horizontalMove = 0f;
		}

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (joystick.Vertical >= 0.5f)
		{
			if (canJump)
			{
				animator.SetBool("IsJumping", true);
				Jump();
			}
		}

	}
	private void CheckGround()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

		if (rb.velocity.y <= 0.0f)
		{
			isJumping = false;
		}

		if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
		{
			OnLanding();

			canJump = true;
		}
	}

	private void SlopeCheck()
	{
		Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

		SlopeCheckHorizontal(checkPos);
		SlopeCheckVertical(checkPos);
	}

	private void SlopeCheckHorizontal(Vector2 checkPos)
	{
		RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
		RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

		if (slopeHitFront)
		{
			isOnSlope = true;

			slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

		}
		else if (slopeHitBack)
		{
			isOnSlope = true;

			slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
		}
		else
		{
			slopeSideAngle = 0.0f;
			isOnSlope = false;
		}

	}

	private void SlopeCheckVertical(Vector2 checkPos)
	{
		RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

		if (hit)
		{
			slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			if (slopeDownAngle != lastSlopeAngle)
			{
				isOnSlope = true;
			}

			lastSlopeAngle = slopeDownAngle;

			Debug.Log(slopeDownAngle);

			Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
			Debug.DrawRay(hit.point, hit.normal, Color.green);
		}

		if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
		{
			canWalkOnSlope = false;
		}
		else
		{
			canWalkOnSlope = true;
		}

		if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
		{
			rb.sharedMaterial = fullFriction;
		}
		else
		{
			rb.sharedMaterial = noFriction;
		}
	}

	private void Jump()
	{
		//Vector3 velocity = rb.velocity;
		//velocity.y = playerStats.JumpPower;
		//rb.velocity = velocity;

		rb.velocity = new Vector2(rb.velocity.x, 0f);
		rb.AddForce(Vector2.up * playerStats.JumpPower, ForceMode2D.Impulse);

		//rb.velocity = new Vector2(rb.velocity.x, 0f);
		//rb.AddForce(Vector2.up * playerStats.JumpPower, ForceMode2D.Impulse);

		//Vector3 jump = new Vector3(0, playerStats.JumpPower, 0);
		//rb.AddForce(jump, ForceMode2D.Impulse);

		canJump = false;
		isJumping = true;

		//newVelocity.Set(0.0f, 0.0f);
		//rb.velocity = newVelocity;
		//newForce.Set(0.0f, playerStats.JumpPower);
		//rb.AddForce(newForce, ForceMode2D.Impulse);
	}

	private void ApplyMovement()
	{
		horizontalMove *= Time.fixedDeltaTime * 10f;
		Vector3 targetVelocity = new Vector3();

		if (isGrounded && !isOnSlope && !isJumping) //if not on slope
		{
			targetVelocity = new Vector2(horizontalMove, rb.velocity.y);
		}
		else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
		{
			targetVelocity = new Vector2(-horizontalMove * slopeNormalPerp.x, -horizontalMove * slopeNormalPerp.y);
		}
		else if (!isGrounded) //If in air
		{
			targetVelocity = new Vector2(horizontalMove, rb.velocity.y);
		}

		// Apply velocity to player
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
	}

	private void Flip()
	{
		facingDirection *= -1;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Terrain"))
		{
			isGrounded = true;
		}
	}

}