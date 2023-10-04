using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelGame : MonoBehaviour
{
    //code load level but not have many level ,not auto
    // public Button[] lvlButtons;
    // public Sprite lock_image;

    // void Start()
    // {
    //     int level=PlayerPrefs.GetInt("level",2);
    //     for(int i=0;i<lvlButtons.Length;i++)
    //     {
    //         lvlButtons[i].GetComponent<Image>().sprite=lock_image;
    //         if(i+2>level)
    //         {
    //            // lvlButtons[i].GetComponent<Image>().sprite=lock_image;
    //             //lvlButtons[i].GetComponent<Button>().interactable=false;
    //             lvlButtons[i].interactable=false;
    //         }
    //     }
    // }
    // public void level_onclick(int a)
    // {
        
    //     SceneManager.LoadScene(a.ToString());
    // }
    public void ResetLevels()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
