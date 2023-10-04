using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStumpObject : MonoBehaviour
{
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BackGround"))
        {
            Destroy(gameObject);
        }
    }
}
