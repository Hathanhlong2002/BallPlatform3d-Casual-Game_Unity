using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPos : MonoBehaviour
{
    public GameObject spawnee;
    public int countBalls;
    public List<GameObject> balls=new List<GameObject>();
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public Transform Balls;
	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
	}
	void Awake()
    {
        
    }
    public void SpawnObject() {
        
        // Instantiate(spawnee, transform.position, transform.rotation);
        GameObject temp=Instantiate(spawnee, transform.position+new Vector3(Random.Range(-3f,3f),0,Random.Range(-3f,3f)), transform.rotation);
        temp.transform.SetParent(Balls);
        balls.Add(temp);
        if(balls.Count>=countBalls)
        {
            stopSpawning=true;
            DeSpawn();
        }
        
        
    }
    void DeSpawn()
    {
        if(stopSpawning) {
            CancelInvoke("SpawnObject");
        }
    }
    void Update()
    {
        BallsDie();
    }
    void BallsDie()
    {
        for(int i=0;i<balls.Count;i++)
        {
            if(balls[i].transform.position.y<=-6)
            {
               
                balls.RemoveAt(i);
                
                
            }
        }
    }
}
