using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypointIndex=0;
    public static WaypointFollow instance;
    public float speed=1f;


    private void Awake()
    {
        instance=this;
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position,waypoints[currentWaypointIndex].transform.position)<0.1f)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex>=waypoints.Length)
            {
                currentWaypointIndex=0;
            }
        }
        transform.position=Vector3.MoveTowards(transform.position,waypoints[currentWaypointIndex].transform.position,speed*Time.deltaTime);
    }

}
