using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    private Vector3 offset;
    void Start()
    {
        offset=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(target.position.x+offset.x,target.position.y+offset.y,target.position.z+offset.z);
    }
}
