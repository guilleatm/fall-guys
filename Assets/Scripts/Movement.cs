using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour {


	public NetworkVariable<Vector3> n_Position = new NetworkVariable<Vector3>();


	Vector3 m_Input = Vector3.zero;
	float m_Speed = 7f;


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
		rb.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
		n_Position.Value = transform.position;

	}
	void ClientUpdate() {
		
		transform.position = n_Position.Value;



		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Move_ServerRpc(input);


	}


	[ServerRpc]
	void Move_ServerRpc(Vector3 input) {

		m_Input = input;

	}




}
