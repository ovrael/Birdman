using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] bool drawGizmos = true;

	[Header("Movement")]
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private float slopeCheckDistance;
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[Header("GroundCheck")]
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private float k_GroundedRadius = .05f;                     // Radius of the overlap circle to determine if grounded
	[Header("Materials")]
	[SerializeField] private PhysicsMaterial2D noFrictionMaterial;              // Material used when jumping to avoid sticking to walls
	[SerializeField] private PhysicsMaterial2D fullFrictionMaterial;            // Material used when walking

	private bool m_Grounded;                                                    // Whether or not the player is grounded.
	private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
	private bool isOnSlope;

	private float movemenet;
	private float slopeDownAngle;
	private float slopeSideAngle;
	private float lastSlopeAngle;

	private CapsuleCollider2D capsuleCollider;
	private Rigidbody2D m_Rigidbody2D;
	private Vector2 capsuleColliderSize;
	private Vector2 slopeNormalPerp;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();


		capsuleCollider = GetComponent<CapsuleCollider2D>();

		capsuleColliderSize = capsuleCollider.size;

	}

	private void Update()
	{
		SlopeCheck();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;

				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	public void Move(float move, float jumpForce, bool jump, out bool isGrounded)
	{
		isGrounded = m_Grounded;
		movemenet = move;
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			m_Grounded = false;
			// Add a vertical force to the player.
			Vector3 velocity = m_Rigidbody2D.velocity;
			velocity.y = jumpForce;
			m_Rigidbody2D.velocity = velocity;
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
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
		RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, m_WhatIsGround);
		RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, m_WhatIsGround);

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
		RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, m_WhatIsGround);

		if (hit)
		{
			slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			if (slopeDownAngle != lastSlopeAngle)
			{
				isOnSlope = true;
			}

			lastSlopeAngle = slopeDownAngle;

			Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
			Debug.DrawRay(hit.point, hit.normal, Color.green);
		}

		if (isOnSlope && movemenet == 0.0f)
		{
			m_Rigidbody2D.sharedMaterial = fullFrictionMaterial;
		}
		else
		{
			m_Rigidbody2D.sharedMaterial = noFrictionMaterial;
		}
	}
}
