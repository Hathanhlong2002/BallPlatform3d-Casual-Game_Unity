using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BallPlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float movementSpeed = 5f;
    private bool isDead=false;
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        PlayerPrefs.SetFloat("RespawnPosX", 0f);
        PlayerPrefs.SetFloat("RespawnPosY", 0f);
        PlayerPrefs.SetFloat("RespawnPosZ", 0f);
        PlayerPrefs.Save();
        rb.isKinematic=true;
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.K))
        {
            rb.isKinematic=true;
        }
        else
        {
            rb.isKinematic=false;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed); 
        }
        if (isDead)
        {
            ItemCollector.instance.hear-=1;
            ItemCollector.instance.hearthText.text="Hearth: "+ItemCollector.instance.hear;
            StartCoroutine(Delay(1f));
            RespawnCharacter();
        }
    }
    //Xử lý trạng thái chết
    private void RespawnCharacter()
    {
        if(ItemCollector.instance.hear<0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            isDead = false;
            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("RespawnPosX"),PlayerPrefs.GetFloat("RespawnPosY")+1.0f,PlayerPrefs.GetFloat("RespawnPosZ"));
            gameObject.transform.position = pos;
        }
    }

    //Xử lý va chạm với các vật thể
    private void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("Coin"))
		{
			AudioManager.instance.audioSource.clip=AudioManager.instance.GetCoin;
			AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
			Destroy(other.gameObject);
            ItemCollector.instance.coins+=10;
            int coinsTmp=PlayerPrefs.GetInt("Coin")+ItemCollector.instance.coins;
            
            ItemCollector.instance.coinsText.text="Coins: "+(coinsTmp);
		}
        if(other.CompareTag("Hearth"))
        {
            Destroy(other.gameObject);
            ItemCollector.instance.hear++;
            ItemCollector.instance.hearthText.text="Hearth: "+ItemCollector.instance.hear;
            StartCoroutine(Delay(1f));
            AudioManager.instance.audioSource.clip=AudioManager.instance.GameBonus;
			AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
            
        }
	}
	void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BackGround"))
        {
            rb.isKinematic=true;
            AudioManager.instance.audioSource.clip=AudioManager.instance.clipDie;
			AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.audioSource.clip);
            isDead=true;
			
        }
        if(collision.gameObject.CompareTag("SaveGame"))
        {
            
            PlayerPrefs.SetFloat("RespawnPosX", collision.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("RespawnPosY", collision.gameObject.transform.position.y);
            PlayerPrefs.SetFloat("RespawnPosZ", collision.gameObject.transform.position.z);
            PlayerPrefs.Save();
            
        }

    }
	
    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    
} 
