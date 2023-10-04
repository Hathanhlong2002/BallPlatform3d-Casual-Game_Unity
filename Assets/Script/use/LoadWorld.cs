using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWorld : MonoBehaviour
{
    public GameObject[] Worlds;
    public Transform spawnPosWorld;


    void Start()
    {
        int selectWorldPos=PlayerPrefs.GetInt("SelectWorld");
        GameObject worldPrefab=Worlds[selectWorldPos];
        GameObject player=Instantiate(worldPrefab,spawnPosWorld.transform.position,Quaternion.identity);
    }
}
