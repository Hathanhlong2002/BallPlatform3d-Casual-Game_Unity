using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject rightDoor;
    public GameObject leftDoor;
    public int nextSceneLoad;
    private bool isFinished = false;

    private void Awake() 
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFinished) return;

        if (other.gameObject.CompareTag("Player") || other.gameObject.GetComponent<BallPlayerMovement>() != null)
        {
            StartCoroutine(VictorySequence());
        }
    }

    private IEnumerator VictorySequence()
    {
        isFinished = true;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySfx(AudioManager.instance.clipVictory);
        }

        if (rightDoor != null) rightDoor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        if (leftDoor != null) leftDoor.transform.rotation = Quaternion.Euler(0f, 270f, 0f);

        yield return new WaitForSeconds(2.0f);

        if (GameManager.instance != null && GameManager.instance.VictoryDialog != null)
        {
            GameManager.instance.VictoryDialog.SetActive(true);
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level ") && int.TryParse(sceneName.Substring(6), out int currentLvl))
        {
            int unlocked = PlayerPrefs.GetInt("unlockedLevel", 1);
            if (currentLvl >= unlocked)
            {
                PlayerPrefs.SetInt("unlockedLevel", currentLvl + 1);
            }
            PlayerPrefs.SetInt("level", currentLvl);
            PlayerPrefs.Save();
        }
    }
}
