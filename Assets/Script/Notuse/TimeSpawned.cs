using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpawned : MonoBehaviour
{
    public GameObject Obj;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    int count=5;
    public Transform paren;
	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnObject", spawnTime, spawnDelay);

	}
	
    public void SpawnObject() {
        
        GameObject tmp=Instantiate(Obj, transform.position+new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)), transform.rotation);
        tmp.transform.SetParent(paren);
        if(count==0) {
            Despawn();
        }
        count--;
        
    }
    void Despawn()
    {
        if(stopSpawning) {
            CancelInvoke("SpawnObject");
        }
    }
}
