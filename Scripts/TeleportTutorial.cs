using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportTutorial : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
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
