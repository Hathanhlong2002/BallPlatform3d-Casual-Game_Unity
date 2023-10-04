using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Next : MonoBehaviour
{
    public int nextSceneLoad;
    void Start()
    {
        
        nextSceneLoad=SceneManager.GetActiveScene().buildIndex+1;
        
    }
    public void NextLevel()
    {
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")+ItemCollector.instance.coins);
        }
        SceneManager.LoadScene(nextSceneLoad);
    }
}
