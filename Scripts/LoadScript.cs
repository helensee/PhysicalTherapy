using UnityEngine;
using System.Collections;

public class LoadScript : MonoBehaviour {
	
	public void loadScene(string name){
		Debug.Log ("HI!");
		Application.LoadLevel(name);

	}

}
