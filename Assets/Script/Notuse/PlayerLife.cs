using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerLife : MonoBehaviour
{
    public GameObject health;
    public GameObject saveGame;
    private int ColiderTrig;
    public bool isDead=false;
    public static PlayerLife instance; 
    public int hearth;
    public float pX;
    public float pY;
    public float pZ;

    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
    }
    void Start()
    {
        if(PlayerPrefs.GetInt("ColiderTrig")==1)
        {
            gameObject.transform.position=new Vector3(pX,pY,pZ);
            ItemCollector.instance.hear=hearth;
            ItemCollector.instance.hearthText.text="Hearth: "+ItemCollector.instance.hear;
        }
    }
    private void Update() {
        if(transform.position.y<=-6f && !isDead)
        {
            Die();
            
        }
    }
    void OnCollisionEnter(Collision collission)
    {
        if(collission.gameObject.CompareTag("EnemyBody"))
        {
            Die();
            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SaveGame"))
        {
            PlayerPrefs.SetInt("ColiderTrig",1);
            
            // Debug.Log("Save");
            

            // px=PlayerPrefs.GetFloat("pX");
            // py=PlayerPrefs.GetFloat("pY");
            // pz=PlayerPrefs.GetFloat("pZ");
            // // PlayerPrefs.SetFloat("pX",gameObject.transform.position.x);
            // // PlayerPrefs.SetFloat("pY",gameObject.transform.position.y);
            // // PlayerPrefs.SetFloat("pZ",gameObject.transform.position.z);
            // // PlayerPrefs.SetInt("Saved",1);
            // PlayerPrefs.SetFloat("pX",gameObject.transform.position.x);
            // PlayerPrefs.SetFloat("pY",gameObject.transform.position.y);
            // PlayerPrefs.SetFloat("pZ",gameObject.transform.position.z);
            // PlayerPrefs.Save();
            // Debug.Log(pX+" "+pY+" "+pZ);
            Debug.Log(ColiderTrig);
        }
    }
    public void Die()
    {
        
        GetComponent<MeshRenderer>().enabled=false;
        GetComponent<Rigidbody>().isKinematic=true;
        GetComponent<BallPlayerMovement>().enabled=false;
        GetComponent<Player>().enabled=false;
        health.SetActive(false);
        isDead=true;
        
        Invoke(nameof(ReloadLevel),2f);
        
        
    }
    void ReloadLevel()
    {

        //  transform.position = new Vector3(PlayerPrefs.GetFloat("pX"), PlayerPrefs.GetFloat("pY"), PlayerPrefs.GetFloat("pZ"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // if(ColiderTrig==1)
        // {
        //     transform.position = new Vector3(PlayerPrefs.GetFloat("pX"), PlayerPrefs.GetFloat("pY"), PlayerPrefs.GetFloat("pZ"));
        //     // gameObject.transform.position=saveGame.transform.position;
        // }
        // gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("pX"), PlayerPrefs.GetFloat("pY"), PlayerPrefs.GetFloat("pZ"));
        // gameObject.transform.position=new Vector3(6,2,50);
    }
    
}
