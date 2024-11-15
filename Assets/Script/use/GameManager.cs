using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text levelText;
    public GameObject VictoryDialog;
    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        VictoryDialog.SetActive(false);
        levelText.text=SceneManager.GetActiveScene().name;
        
    }
    public void GamePlay()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
 
}
