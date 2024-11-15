using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public GameObject setting;
    private string loading= "Loading";
    private void Start()
    {
        if (setting.activeSelf) DeactiveSetting();
    }
    public void ActiveSetting()
    {
        setting.SetActive(true);
    }    
    public void DeactiveSetting()
    {
        setting.SetActive(false);
    }
    public void Menu()
    {
        SceneManager.LoadScene(loading);

    }
   


    public void QuitGame()
    {
        Application.Quit();
    }


}
