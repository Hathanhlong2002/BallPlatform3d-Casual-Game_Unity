using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollector : Singleton<ItemCollector>
{
    public Text coinsText;
    public Text hearthText;
    public int coins=0;
    public int heart=0;
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
        hearthText.text="Hearth: "+heart;
    }
   
}
