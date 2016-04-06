using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportTutorial : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if(other.name == "CastleHallway")
        {
            SceneManager.LoadScene("Scenes/CastleHallway");
        }
        else if (other.name == "Teleport_Fireball")
        {
            SceneManager.LoadScene("Scenes/TutorialFireball_Cheryl");
        } else if (other.name == "Teleport_ShootEarth")
        {
            SceneManager.LoadScene("Scenes/TutorialShootEarth_Adam");
        }
        else if(other.name == "OutsideCastle")
        {
            SceneManager.LoadScene("Scenes/OfficialWorld");
        }
        
    }
}
