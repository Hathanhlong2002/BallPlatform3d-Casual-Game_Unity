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
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + (ItemCollector.instance != null ? ItemCollector.instance.coins : 0));
        }

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.StartsWith("Level ") && int.TryParse(currentScene.Substring(6), out int currentLvl))
        {
            int nextLvl = currentLvl + 1;
            if (nextLvl <= 50)
            {
                SceneManager.LoadScene("Level " + nextLvl);
                return;
            }
        }

        // Đã hoàn thành màn 50 hoặc không có màn tiếp theo -> quay về LevelSelection
        SceneManager.LoadScene("LevelSelection");
    }
}
