using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject rightDoor;
    public GameObject leftDoor;
    public int nextSceneLoad;
    private void Awake() 
    {
        nextSceneLoad=SceneManager.GetActiveScene().buildIndex+1;
    }
    void Start()
    {
        nextSceneLoad=SceneManager.GetActiveScene().buildIndex+1;   
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            
            AudioManager.instance.audioSource.clip=AudioManager.instance.clipVictory;
			AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
            rightDoor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);;
            leftDoor.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            StartCoroutine(Delay(2.0f));
            GameManager.instance.VictoryDialog.SetActive(true);
            if(nextSceneLoad-2>PlayerPrefs.GetInt("level") )
            {
                PlayerPrefs.SetInt("unlockedLevel",nextSceneLoad-1);
                PlayerPrefs.SetInt("level",PlayerPrefs.GetInt("level")+1);
                
                Debug.Log("level"+PlayerPrefs.GetInt("level"));
                Debug.Log("unlockedLevel"+PlayerPrefs.GetInt("unlockedLevel"));
                Debug.Log("nextsceneLoad"+nextSceneLoad);
            }
        }

    }
    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
