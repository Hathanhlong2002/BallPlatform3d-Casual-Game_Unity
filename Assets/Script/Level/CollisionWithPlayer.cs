using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithPlayer : MonoBehaviour
{
    AudioManager audioManager;
    ItemCollector collector;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        collector = ItemCollector.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.audioSource.clip = audioManager.GetCoin;
            audioManager.audioSource.PlayOneShot(audioManager.audioSource.clip);
            Destroy(gameObject);
            collector.coins += 10;
            int coinsTmp = PlayerPrefs.GetInt("Coin") + collector.coins;

            collector.coinsText.text = "Coins: " + (coinsTmp);
        }
    }
}
