using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScript : MonoBehaviour {

	public void loadScene(string name){
		Debug.Log ("HI!");
        SceneManager.LoadScene(name);
    }

}
