using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed=5f;
    public Rigidbody rigid;
    private void Start()
    {
        rigid=gameObject.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(Input.GetAxis("Horizontal")<0)
        {
            rigid.AddForce(Vector3.left*speed);
        }
        else if(Input.GetAxis("Horizontal")>0)
        {
            rigid.AddForce(Vector3.right*speed);
        }
        if(Input.GetAxis("Vertical")<0)
        {
            rigid.AddForce(-Vector3.forward*speed);
        }
        else if(Input.GetAxis("Vertical")>0)
        {
            rigid.AddForce(Vector3.forward*speed);
        }
    }
}
