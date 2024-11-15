using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visible : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        // gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
    }
    // void OnTriggerEnter(Collider other)
    // {
    //     if(other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Player");
            
            
            
    //     }
    // }
}
