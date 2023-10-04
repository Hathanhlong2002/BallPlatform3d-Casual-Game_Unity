using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject setting;
    public Text levelText;
    public GameObject VictoryDialog;
    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        VictoryDialog.SetActive(false);
        setting.SetActive(false);
        levelText.text=SceneManager.GetActiveScene().name;
        
    }
    public void GamePlay()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Tutorial()
    {
        setting.SetActive(true);
    }
    public void CloseDialog()
    {
        setting.SetActive(false);
    }
    public void Menu()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    
    public void SkinBalls()
    {
        SceneManager.LoadScene("PlayerSelection");
    }
    public void OverWorld()
    {
        SceneManager.LoadScene("OverWorldSelection");
    }
}
