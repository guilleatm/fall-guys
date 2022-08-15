using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {


	NetworkVariable<bool> jumping = new NetworkVariable<bool>();

	Vector3 m_Input = Vector3.zero;
	float m_Speed = 7f;
	float m_RotationSpeed = 260; // degrees / second
	float m_JumpForce = 10;

	public float forwardInput => m_Input.z;


	bool canMove => jumping.Value == false;
	bool canJump => jumping.Value == false;


	Rigidbody rb;

	void Start() {


		if (NetworkManager.Singleton.IsServer)
			rb = GetComponent<Rigidbody>();


	}

	void Update() {


		if (NetworkManager.Singleton.IsServer) ServerUpdate();
		if (NetworkManager.Singleton.IsClient) ClientUpdate();
		
		
	}

	public override void OnNetworkSpawn() {
		
		jumping.Value = false;
		if (IsClient)
			jumping.OnValueChanged += OnJumpingChanged;
		

	}

	void OnJumpingChanged(bool oldValue, bool newValue) {
		Debug.Log("jumping changed");
	}

	void ServerUpdate() {

	}

	void ClientUpdate() {
		// INPUT

		m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		bool jumpInput = Input.GetKeyDown("space");


		// SERVER RPC CALLS
		if (IsOwner) {


			if (canJump && jumpInput) {
				Jump_ServerRpc();
			}

			if (canMove) Move_ServerRpc(m_Input);


		}

	}

	[ServerRpc]
	void Move_ServerRpc(Vector3 input) {

		m_Input = input;

		// Movement
		Vector3 forward = rb.transform.forward * Mathf.Max(0, input.z);
		rb.MovePosition(transform.position + forward * Time.deltaTime * m_Speed);

		// Rotation
		Vector3 rotation = new Vector3(0, input.x, 0);
		Vector3 deltaRotation = rotation * m_RotationSpeed * Time.deltaTime;
		
		rb.MoveRotation(rb.rotation * Quaternion.Euler(deltaRotation));


	}


	[ServerRpc]
	void Jump_ServerRpc() {
		StartCoroutine(StartJump());
	}

	// Server
	IEnumerator StartJump() {
		jumping.Value = true;

		GetComponent<PlayerAnimationController>().StartJumpAnimation();

		yield return new WaitForSeconds(.1f);


		Vector3 forceDirection = (rb.transform.forward.normalized + Vector3.up).normalized;
		Vector3 jumpForce = forceDirection *  m_JumpForce;
		rb.AddForce(jumpForce, ForceMode.VelocityChange);

		yield return new WaitForSeconds(1.2f);


		jumping.Value = false;
	}

}
