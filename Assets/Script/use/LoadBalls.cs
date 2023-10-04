using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBalls : MonoBehaviour
{
    public GameObject[] ballsPrefabs;
    public Transform spawnPosBalls;


    void Start()
    {
        int selectBallPos=PlayerPrefs.GetInt("SelectBall");
        GameObject ballPrefab=ballsPrefabs[selectBallPos];
        GameObject player=Instantiate(ballPrefab,spawnPosBalls.transform.position,Quaternion.identity);
        spawnPosBalls.SetParent(player.transform);
    }

    
}
