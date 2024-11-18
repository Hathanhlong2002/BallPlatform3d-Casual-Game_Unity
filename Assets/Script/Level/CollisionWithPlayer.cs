using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.audioSource.clip = AudioManager.instance.GetCoin;
            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
            Destroy(gameObject);
            ItemCollector.instance.coins += 10;
            int coinsTmp = PlayerPrefs.GetInt("Coin") + ItemCollector.instance.coins;

            ItemCollector.instance.coinsText.text = "Coins: " + (coinsTmp);
        }
    }
}
