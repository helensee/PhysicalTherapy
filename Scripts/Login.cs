using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login : MonoBehaviour {
	public GameObject input;
	public Text error;

	public void checkLogin() {
		Debug.Log (Application.persistentDataPath);
		InputField hi = input.GetComponent<InputField> ();
		Debug.Log (hi.text);
		//do login stuff - loading scene
		UserControl.instance.test();

		bool success;
		success = UserControl.instance.Load(hi.text);

		if (success) {
			//load new scene
			Application.LoadLevel("MainMenu");
		} else {
			//display error
			error.text = "Invalid User";
			//error.text = Application.persistentDataPath;
		}
	}

	public void newUser() {
		int userid = UserControl.instance.AddUser ();
		error.text = "Your user ID is " + userid + ". Please log in.";
		//UserControl.instance.Save (userid); already saved in usercontrol
	}
}
