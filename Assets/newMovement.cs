using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newMovement : MonoBehaviour
{
	//diplacement and camera
	[SerializeField] private CharacterController controller;
	[SerializeField] private Transform cam;
	[SerializeField] private float wspeed = 6f;
	[SerializeField] private float rspeed = 18f;
	[SerializeField] private float turnSmoothTime = 0.1f;
	private float speed = 6f;
	float turnSmoothVelocity;

	//gravity and jump
	Vector3 velocity;
	float gravity = -9.81f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private float groundDistance = 0.2f;
	[SerializeField] private float jumpHeight = 10.0f;
	bool isGrounded;

	//animator
	[SerializeField] private Animator m_Animator;
	[SerializeField] private float m_RunCycleLegOffset = 0.2f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	private float m_TurnAmount;
	private float m_ForwardAmount;
	const float k_Half = 0.5f;


	

	// Update is called once per frame
	void Update()
	{

		Gravity();
		Diplacement();
		Run();
		Jump();

		void Gravity(){
			isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
			if (isGrounded && velocity.y < 0) {
				velocity.y = -0.4f;
			}
			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);
		}

		void Diplacement(){
			float horizontal = Input.GetAxisRaw("Horizontal");
			float vertical = Input.GetAxisRaw("Vertical");
			float targetAngle = 0f;
			float angle = 0f;
			Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; 

			if(direction.magnitude >= 0.1f)
			{
				targetAngle = cam.eulerAngles.y;
				angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);

				direction = transform.TransformDirection(direction);

				controller.Move(direction * speed * Time.deltaTime);

				//m_TurnAmount = Mathf.Atan2(direction.x, direction.z);
			}
			m_TurnAmount = 0f;
			m_ForwardAmount = direction.magnitude * speed / rspeed;


			UpdateAnimator(direction);
		}

		void Run(){
			if (Input.GetAxisRaw("Vertical") == 1.0f && Input.GetKey("left shift") && isGrounded) {
				speed = rspeed;
			}
			else {
				speed = wspeed;
			}

		}

		void Jump() {
			if (isGrounded && Input.GetKey("space")) {
				velocity.y = jumpHeight;
			}
		}


	}

	void UpdateAnimator(Vector3 move)
	{
		// update the animator parameters
		m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", m_TurnAmount, turnSmoothTime, Time.deltaTime);
		//m_Animator.SetFloat("Turn", 0f, 0.1f, Time.deltaTime);
		m_Animator.SetBool("Crouch", false);
		m_Animator.SetBool("OnGround", isGrounded);
		if (!isGrounded)
		{
			m_Animator.SetFloat("Jump", velocity.y);
		}

		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
		float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
		if (isGrounded)
		{
			m_Animator.SetFloat("JumpLeg", jumpLeg);
		}

		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (isGrounded && move.magnitude > 0)
		{
			m_Animator.speed = m_AnimSpeedMultiplier;
		}
		else
		{
			// don't use that while airborne
			m_Animator.speed = 1;
		}
	}
}

