using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class UserControl : MonoBehaviour {
	public int user_id;
	public string username;
	public int current_level;
	public int overall_level;
	public float overall_experience;
	public float earth_experience;
	public float fire_experience;
	public float wind_experience;
	public float water_experience;

	public static UserControl instance = null;
	public List<String> userIDs = new List<String> ();

	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

        Setup();
	}

	// Update is called once per frame
	void Update () {

	}

	public int AddUser() {
		String newUser = userIDs.Count + "";
		userIDs.Add (newUser);
		Debug.Log ("There are " + userIDs.Count + " users!");
		Save(userIDs.Count - 1);
		return userIDs.Count - 1;
	}

	public void Save(int userID) {
		BinaryFormatter bf = new BinaryFormatter ();

		//current user
		FileStream file = File.Open (Application.persistentDataPath + "/" + userID + ".txt", FileMode.Create);

		UserData data = new UserData ();
		data.user_id = user_id;
		data.username = username;
		data.current_level = current_level;
		data.overall_level = overall_level;
		data.earth_experience = earth_experience;
		data.fire_experience = fire_experience;
		data.wind_experience = wind_experience;
		data.water_experience = water_experience;

		bf.Serialize (file, data);
		file.Close ();

		//metadata
		FileStream file2 = File.Open (Application.persistentDataPath + "/metadata.txt", FileMode.Create);
		Metadata meta = new Metadata ();
		meta.userIDs = userIDs;
		bf.Serialize (file2, meta);
		file2.Close ();

	}
	public void test() {
		Debug.Log ("in usercontrol");
	}

    public void Setup()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //load metadata
        if(File.Exists(Application.persistentDataPath + "/metadata.txt"))
        {
            FileStream file2 = File.Open(Application.persistentDataPath + "/metadata.txt", FileMode.Open);
            Metadata meta = (Metadata)bf.Deserialize(file2);
            file2.Close();

            userIDs = meta.userIDs;
            Debug.Log("Load: there are " + userIDs.Count + " users.");
        }
        else
        {
            FileStream file2 = File.Open(Application.persistentDataPath + "/metadata.txt", FileMode.Create);
            file2.Close();
        }
        
    }

	public bool Load(String id) {
		if (File.Exists (Application.persistentDataPath + "/" + id + ".txt")) {
			BinaryFormatter bf = new BinaryFormatter ();

			//load currrent user
			FileStream file = File.Open (Application.persistentDataPath + "/" + id + ".txt", FileMode.Open);
			UserData data = (UserData)bf.Deserialize (file);
			file.Close ();

			user_id = data.user_id;
			username = data.username;
			current_level = data.current_level;
			overall_level = data.overall_level;
			overall_experience = data.overall_experience;
			earth_experience = data.earth_experience;
			fire_experience = data.fire_experience;
			wind_experience = data.wind_experience;
			water_experience = data.water_experience;

			
			return true;
		} else
			return false;
	}
}

[Serializable]
class UserData {
	public int user_id;
	public string username;
	public int current_level;
	public int overall_level;
	public float overall_experience;
	public float earth_experience;
	public float fire_experience;
	public float wind_experience;
	public float water_experience;
}

[Serializable]
class Metadata {
	public List<String> userIDs;
}