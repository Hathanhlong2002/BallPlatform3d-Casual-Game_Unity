using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform targetparent;
    void Start() 
    {
        var groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var groundObject in groundObjects) 
        {
            var coin = Instantiate(coinPrefab, groundObject.transform.position+new Vector3(0f,0.3f,0f), groundObject.transform.rotation);
            coin.transform.SetParent(targetparent);
        }
        var bridgeObjects = GameObject.FindGameObjectsWithTag("Bridge");
        
        foreach (var bridgeObject in bridgeObjects) 
        {
            var coin = Instantiate(coinPrefab, bridgeObject.transform.position+new Vector3(0f,0.9f,0f), bridgeObject.transform.rotation);
            coin.transform.SetParent(targetparent);
        }
    }
}

