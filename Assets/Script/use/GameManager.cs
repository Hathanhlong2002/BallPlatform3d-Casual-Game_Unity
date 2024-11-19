using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{

    public Text levelText;

    void Start()
    {
        levelText.text="Level: "+ PlayerPrefs.GetInt("currentLevel").ToString();
    }

 
}
