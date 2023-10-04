using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollector : MonoBehaviour
{
    public Text coinsText;
    public Text hearthText;
    public int coins=0;
    public int hear=0;
    public static ItemCollector instance;
    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")+coins);
        }
        
        coinsText.text="Coins: "+PlayerPrefs.GetInt("Coin");
        hearthText.text="Hearth: "+hear;
    }
   
}
