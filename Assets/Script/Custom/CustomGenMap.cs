using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGenMap : MonoBehaviour
{
    public GameObject firstObject;
    public float currentPositionZ;
    public List<ObjectGen> objectGenList;
    public SpawnCoins spawnCoins;
    public ObjectGen endMap;

    public virtual void Awake()
    {

    }
    private void Start()
    {
        if (!objectGenList.Contains(endMap))
        {
            objectGenList.Add(endMap);
        }

        GenListObject();
        spawnCoins.Spawn();
    }
    private void GenListObject()
    {
        for (int i = 0; i < objectGenList.Count; i++)
        {
            ObjectGen tempGen = objectGenList[i];
            GenNewMap(tempGen.obj, tempGen.count, tempGen.distance);
        }
    }
    public void GenNewMap(GameObject objectGen, int count, float distance)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject tmp = Instantiate(objectGen);
            float distanceTmp = distance * (i + 1) + currentPositionZ;
            tmp.transform.SetPositionAndRotation(firstObject.transform.position + new Vector3(0f, i % 2 == 0 ? 0.01f : -0.01f, distanceTmp),
                Quaternion.identity);
            tmp.transform.SetParent(transform, false);
        }
        currentPositionZ += distance * count;

    }
}
