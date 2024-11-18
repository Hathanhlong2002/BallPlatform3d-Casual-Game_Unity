using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BallPlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float movementSpeed = 5f;
    private bool isDead=false;
    private int currentScene;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        rb =GetComponent<Rigidbody>();
        PlayerPrefs.SetFloat("RespawnPosX", transform.position.x);
        PlayerPrefs.SetFloat("RespawnPosY", transform.position.y);
        PlayerPrefs.SetFloat("RespawnPosZ", transform.position.z);
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
            ItemCollector.Instance.heart -= 1;
            ItemCollector.Instance.hearthText.text = "Hearth: " + ItemCollector.Instance.heart;
            RespawnCharacter();
        }
    }
    //Xử lý trạng thái chết
    private void RespawnCharacter()
    {
        if (ItemCollector.Instance.heart < 0)
        {
            SceneManager.LoadScene(currentScene);
        }
        else
        {
            isDead = false;
            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("RespawnPosX"), PlayerPrefs.GetFloat("RespawnPosY") + 1.0f, PlayerPrefs.GetFloat("RespawnPosZ"));
            gameObject.transform.position = pos;
        }
    }


   



} 
