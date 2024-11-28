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
        

        GenListObject(objectGenList);
       
    }
    private void GenListObject(List<ObjectGen> listGen)
    {
        if (listGen.Count > 0 && !listGen.Contains(endMap))
        {
            listGen.Add(endMap);
        }
        for (int i = 0; i < listGen.Count; i++)
        {
            ObjectGen tempGen = listGen[i];
            GenNewMap(tempGen.obj, tempGen.count, tempGen.distance);
        }
        spawnCoins.Spawn();
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
    public void CreateMap()
    {
        GenListObject(UIManager.Instance.newobjectGens);

    }
    public void ResetMap()
    {
        foreach(Transform transformChild in gameObject.transform)
        {
            Destroy(transformChild.gameObject);
        }
        UIManager.Instance.newobjectGens.Clear();
        UIManager.Instance.ViewResultGen();

    }
}
