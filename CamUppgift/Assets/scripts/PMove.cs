using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour {

	[Header("Movement Values")]
	public float PWalkSpeed;
	public float PJumpSpeed;
	public float PGravity;
	public float turnSmoothTime;
	float turnSmoothVel;
	float currentSpeed;
	float currentSpeedSmoothing;
	[Space]

	[Header("Camera")]
	public Transform CameraTransform;

	CharacterController PController;
	Vector3 moveDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
		PController = GetComponent<CharacterController> ();

	}
	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

			if (inputDir != Vector2.zero) {
				float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + CameraTransform.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmoothTime);
			}
			float targetSpeed = PWalkSpeed * inputDir.magnitude;
			currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref currentSpeedSmoothing, turnSmoothTime);
			transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);
			//moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
			//moveDirection *= PWalkSpeed;
		if (PController.isGrounded) {
			if (Input.GetButtonDown ("Jump")) {
				moveDirection.y = PJumpSpeed;
			}
		}

		moveDirection.y -= PGravity * Time.deltaTime;
		PController.Move (moveDirection * Time.deltaTime);

	}
}
