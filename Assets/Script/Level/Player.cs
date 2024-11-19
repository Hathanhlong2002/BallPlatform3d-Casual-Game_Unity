using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
	AudioManager audioManager;
    ItemCollector collector;
    private void Start()
    {
        audioManager = AudioManager.Instance;
        collector = ItemCollector.Instance;
    }
	void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BackGround"))
        {
            audioManager.audioSource.clip=audioManager.clipDie;
			audioManager.audioSource.PlayOneShot(audioManager.audioSource.clip);
			StartCoroutine(LoadSceneWithDelay(0.5f));
        }

        if (collision.gameObject.CompareTag("SaveGame"))
        {

            PlayerPrefs.SetFloat("RespawnPosX", collision.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("RespawnPosY", collision.gameObject.transform.position.y);
            PlayerPrefs.SetFloat("RespawnPosZ", collision.gameObject.transform.position.z);
            PlayerPrefs.Save();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hearth"))
        {
            Destroy(other.gameObject);
            collector.heart++;
            collector.hearthText.text = "Hearth: " + collector.heart;
            audioManager.audioSource.clip = audioManager.GameBonus;
            audioManager.audioSource.PlayOneShot(audioManager.audioSource.clip);

        }
    }
    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
	
}
