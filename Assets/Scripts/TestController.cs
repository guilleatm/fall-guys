using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TestController : MonoBehaviour {
    
	void Start() {
		
		NetworkManager _networkManager = GetComponent<NetworkManager>();


		if (Application.isEditor)
			_networkManager.StartHost();
		else
			_networkManager.StartClient();


		




	}
}
