using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject dialogPanel;
    public GameObject victoryPanel;

    public void OpenDialog()
    {
        dialogPanel.SetActive(true);
        victoryPanel.SetActive(false);
    }
    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
