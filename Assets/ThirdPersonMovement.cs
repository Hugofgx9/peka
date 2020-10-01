using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
	//diplacement and camera
	[SerializeField] private CharacterController controller;
	[SerializeField] private Transform cam;
	[SerializeField] private float wspeed = 6f;
	[SerializeField] private float rspeed = 18f;
	private float speed = 6f;
	[SerializeField] private float turnSmoothTime = 0.1f;
	float turnSmoothVelocity;

	//gravity and jump;
	Vector3 velocity;
	float gravity = -9.81f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private float groundDistance = 0.2f;
	[SerializeField] private float jumpHeight = 10.0f;
	bool isGrounded;

	

	// Update is called once per frame
	void Update()
	{

		Gravity();
		Diplacement();
		Run();
		Jump();
		print(isGrounded);

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
			Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; 

			if(direction.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);

				Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

				controller.Move(moveDir.normalized * speed * Time.deltaTime);

			}
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
}
