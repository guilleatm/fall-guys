using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TestController : MonoBehaviour {
    
	void Start() {
		

		if (Application.isEditor) return;


		NetworkManager _networkManager = GetComponent<NetworkManager>();
		_networkManager.StartClient();




	}
}
