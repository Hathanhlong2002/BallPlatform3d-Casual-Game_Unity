using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject rightDoor;
    public GameObject leftDoor;
    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            audioManager.audioSource.clip= audioManager.clipVictory;
			audioManager.audioSource.PlayOneShot(audioManager.audioSource.clip);
            rightDoor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);;
            leftDoor.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            UIManager.Instance.victoryPanel.SetActive(true);

            if (PlayerPrefs.GetInt("unlockedLevel") == PlayerPrefs.GetInt("currentLevel") )
            {
                PlayerPrefs.SetInt("unlockedLevel", PlayerPrefs.GetInt("currentLevel") + 1);
                PlayerPrefs.SetInt("level",PlayerPrefs.GetInt("level")+1);
            }
        }

    }
  
}
