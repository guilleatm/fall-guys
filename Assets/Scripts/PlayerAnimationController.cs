using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


// ANIMATIONS WILL BE MANAGED IN SERVER ONLY
public class PlayerAnimationController : NetworkBehaviour {

	float idle2runThreshold = .1f;

	Animator animator;
	PlayerMovement playerMovement;


	// void Start() {

	// 	if (NetworkManager.Singleton.IsClient) return;

	// 	animator = GetComponent<Animator>();
	// 	playerMovement = GetComponent<PlayerMovement>();


	// }


	public override void OnNetworkSpawn() {
		
		//if (NetworkManager.Singleton.IsClient) return;

		animator = GetComponent<Animator>();
		playerMovement = GetComponent<PlayerMovement>();
		

	}

	void Update() {
		
		//if (NetworkManager.Singleton.IsClient) return;


		bool running = playerMovement.forwardInput > idle2runThreshold;
		Debug.Log(running);

		animator.SetBool("running", running);


	}

	public void StartJumpAnimation() {
		GetComponent<Animator>().SetTrigger("jump");
	}
}
