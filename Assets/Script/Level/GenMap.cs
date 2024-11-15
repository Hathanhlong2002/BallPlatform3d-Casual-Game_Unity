using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMap : MonoBehaviour
{
    public GameObject firstObject;
    public List<GameObject> objects;

    private void Start()
    {
        GenNewMap();
    }
    public void GenNewMap()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject tmp = Instantiate(objects[0].gameObject);
            tmp.transform.SetPositionAndRotation(firstObject.transform.position+new Vector3(0f,i%2==0?0.01f:-0.01f,6f*i),
                Quaternion.identity);
            tmp.transform.SetParent(transform,false);
        }
    }
}
