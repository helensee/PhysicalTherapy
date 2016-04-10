using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/playerInfo/" + user_id + ".txt", FileMode.Create);

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
	}

	public void Load() {
		if (File.Exists (Application.persistentDataPath + "/playerInfo/" + user_id + ".txt")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo/" + user_id + ".txt", FileMode.Open);
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
		}
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