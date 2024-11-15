using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneMenu : MonoBehaviour
{
    public void MainMenu()
    {
        // SceneManager.LoadScene("TestMenu");
        SceneManager.LoadScene("LevelSelection");
    }
}
