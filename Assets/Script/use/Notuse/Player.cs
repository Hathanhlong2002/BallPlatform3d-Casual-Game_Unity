using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("Coin"))
		{
			AudioManager.instance.audioSource.clip=AudioManager.instance.GetCoin;
			AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
			Destroy(other.gameObject);
		}
	}
	void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BackGround"))
        {
            AudioManager.instance.audioSource.clip=AudioManager.instance.clipDie;
			AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
			StartCoroutine(LoadSceneWithDelay(1f));
        }
    }
	private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
	
}
