using UnityEngine;
using System.Collections;

public class saving : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void callPrint()
    {
        Debug.Log("printbutton...print");
        UserControl.instance.printTCDataToFile(0);
    }

    public void callSave()
    {
        Debug.Log("savebutton...save");
        if (UserControl.instance.time_conf_data == null)
        {
            Debug.Log("null in callsave");
        }
        else
        {
            Debug.Log("not null in callsave, size: " + UserControl.instance.time_conf_data.Count);
        }
        UserControl.instance.Save(0, UserControl.instance.time_conf_data);
    }
}
