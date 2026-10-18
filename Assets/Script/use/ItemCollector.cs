using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollector : MonoBehaviour
{
    public static ItemCollector instance;

    public Text coinsText;
    public Text hearthText;
    public int coins = 0;
    public int hear = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    public void AddHeart(int amount = 1)
    {
        hear += amount;
        UpdateUI();
    }

    public bool DeductHeart()
    {
        hear--;
        UpdateUI();
        return hear < 0;
    }

    public void UpdateUI()
    {
        int totalCoins = PlayerPrefs.GetInt("Coin", 0) + coins;
        if (coinsText != null) coinsText.text = "Coins: " + totalCoins;
        if (hearthText != null) hearthText.text = "Hearth: " + hear;
    }
}
