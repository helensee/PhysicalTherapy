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
    public List<TCData> time_conf_data;

    public static UserControl instance = null;
	public List<String> userIDs = new List<String> ();

	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

        Debug.Log(Application.persistentDataPath);

        Setup();
	}

	// Update is called once per frame
	void Update () {

	}

    public void addTCData(String time, float conf, String gesture)
    {
        Debug.Log("Adding tcdata");
        if(time_conf_data == null)
        {
            Debug.Log("null in add");
            time_conf_data = new List<TCData>();
        }
        else
        {
            Debug.Log("not null in add, size: " + time_conf_data.Count);
        }
        time_conf_data.Add(new TCData(time, conf, gesture));
        Debug.Log("Calling save from adddata");
        //Save(user_id);

    }

    public void printTCDataToFile(int userID)
    {
        Debug.Log("printing to file...");
        if (!File.Exists(Application.persistentDataPath + "/TCDataUser" + userID + ".txt")) { 
            FileStream file = File.Open(Application.persistentDataPath + "/TCDataUser" + userID + ".txt", FileMode.Create);
            file.Close();
        }
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/TCDataUser" + userID + ".txt"))
        {
            Debug.Log("in sw");
            if (time_conf_data == null)
            {
                Debug.Log("its null sob");
                
            }
            if(time_conf_data != null)
            {
                Debug.Log("its not null");
                Debug.Log(time_conf_data.Count);
            }
            /*
            Debug.Log("in sw");
            sw.WriteLine("hello world");
            sw.WriteLine(time_conf_data.Count);
            Debug.Log("yay");
        */    
        /*Debug.Log("in streamwriter, data size: " + time_conf_data.Count);
            for(int i=0; i<time_conf_data.Count; i++)
            {
                TCData tcd = time_conf_data[i];
                Debug.Log(tcd.toString());
                sw.WriteLine(tcd.toString());
            }*/
        }

    }

    public int AddUser() {
		String newUser = userIDs.Count + "";
		userIDs.Add (newUser);
		Debug.Log ("There are " + userIDs.Count + " users!");
		Save(userIDs.Count - 1, new List<TCData>());
		return userIDs.Count - 1;
	}

	public void Save(int userID, List<TCData> tcd_data) {
        Debug.Log("saving...");
        if (tcd_data == null)
        {
            Debug.Log("its null before serializing");
        }
        else {
            Debug.Log("its not null before serializaing");
            Debug.Log(time_conf_data.Count);
        }
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
        data.time_conf_data = time_conf_data;

		bf.Serialize (file, data);
		file.Close ();
        if (time_conf_data == null)
        {
            Debug.Log("its null in save");
        }
        else
            Debug.Log("its not nul in save");
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
            time_conf_data = data.time_conf_data;

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
    public List<TCData> time_conf_data;

    public UserData()
    {
        time_conf_data = new List<TCData>();
    }
}

[Serializable]
class Metadata {
	public List<String> userIDs;
}

public class TCData
{
    public String timestamp;
    public float confidence;
    public String gesture;

    public TCData(String time, float conf, String gesture)
    {
        timestamp = time;
        confidence = conf;
        this.gesture = gesture;
    }

    public String toString()
    {
        return "Timestamp: " + timestamp + "; Gesture: " + gesture+ "; Confidence: " + confidence;
    }
}