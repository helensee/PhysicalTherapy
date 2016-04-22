using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportTutorial : MonoBehaviour {

    public GameObject kyleHalp;
    public bool visible;
    public GameObject kyleplz;

    void Start()
    {
        visible = true;
        kyleplz = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && visible == false)
        {
            kyleplz = Instantiate(kyleHalp, GameObject.Find("kyleboo").transform.position, Quaternion.identity) as GameObject;

            visible = true;

            Debug.Log("hi kyle");
        }
        else if(Input.GetKeyDown(KeyCode.H) && visible == true)
        {
            Destroy(kyleplz);
            visible = false;
            Debug.Log("bye kyle");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            visible = false;
            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "CastleHallway")
        {
            SceneManager.LoadScene("Scenes/CastleHallway");
        }
        else if (other.name == "Teleport_Fireball")
        {
            SceneManager.LoadScene("Scenes/TutorialFireball");
        } else if (other.name == "Teleport_ShootEarth")
        {
            SceneManager.LoadScene("Scenes/TutorialShootingRock");
        }
        else if(other.name == "OutsideCastle")
        {
            SceneManager.LoadScene("Scenes/OfficialWorld");
        }
        
    }
}
