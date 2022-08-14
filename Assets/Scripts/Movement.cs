using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour {


	public NetworkVariable<Vector3> n_Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> n_Rotation = new NetworkVariable<Quaternion>();



	Vector3 m_Input = Vector3.zero;
	float m_Speed = 7f;
	float m_RotationSpeed = 260; // degrees / second


	Rigidbody rb;

	public override void OnNetworkSpawn() {
	}

	void Start() {

		if (NetworkManager.IsServer)
			rb = GetComponent<Rigidbody>();

	}

	void Update() {
		
		// #D Singleton?
		if (NetworkManager.IsServer) ServerUpdate();
		if (NetworkManager.IsClient) ClientUpdate();


	}


	void ServerUpdate() {
		
		// Movement
		Vector3 forward = rb.transform.forward * Mathf.Max(0, m_Input.z);
		rb.MovePosition(transform.position + forward * Time.deltaTime * m_Speed);

		// Rotation
		Vector3 rotation = new Vector3(0, m_Input.x, 0);
		Vector3 deltaRotation = rotation * m_RotationSpeed * Time.deltaTime;
		
		rb.MoveRotation(rb.rotation * Quaternion.Euler(deltaRotation));
		

		n_Position.Value = transform.position;
		n_Rotation.Value = rb.rotation;



	}
	void ClientUpdate() {
		
		transform.position = n_Position.Value;
		transform.rotation = n_Rotation.Value;


		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Move_ServerRpc(input);


	}


	[ServerRpc]
	void Move_ServerRpc(Vector3 input) {

		m_Input = input;

	}




}
