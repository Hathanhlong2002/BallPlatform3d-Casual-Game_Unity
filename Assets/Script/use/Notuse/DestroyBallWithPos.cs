using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBallWithPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroyGameObj()
    {
        if(transform.position.y<=-16)
            {        
                Destroy(gameObject);  
            }
    }
}
