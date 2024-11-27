using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom : MonoBehaviour
{
    public GameObject objectPanel;
    public GameObject resultGenPanel;
    private bool isObjectPanelActive = false;
    [SerializeField] List<ObjectGen> objectGens;


    private void Start()
    {
        SettingCustom();
        Getlist();
    }
    private void Getlist()
    {
        GameObject obj=UIManager.Instance.objectGen;
        Transform parent=UIManager.Instance.contentObjectGen;
        for(int i=0;i< objectGens.Count;i++)
        {
            GameObject tmp = Instantiate(obj);
            tmp.transform.SetParent(parent.transform,false);
            ObjectData data= tmp.GetComponent<ObjectData>();
            data.nameObject.text = objectGens[i].obj.name;
        }
    }
    public void SettingCustom()
    {
        isObjectPanelActive = !isObjectPanelActive;
        objectPanel.SetActive(isObjectPanelActive);
        resultGenPanel.SetActive(isObjectPanelActive);
    }
}
