using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            int index = i;
            data.increase.GetComponent<Button>().onClick.AddListener(() => ClickAddObjectGen(index));
            data.decrease.GetComponent<Button>().onClick.AddListener(() => ClickDecreaseObjectGen(index));
        }
    }
    public void SettingCustom()
    {
        isObjectPanelActive = !isObjectPanelActive;
        objectPanel.SetActive(isObjectPanelActive);
        resultGenPanel.SetActive(isObjectPanelActive);
    }
    public void ClickAddObjectGen(int index)
    {
        UIManager.Instance.newobjectGens.Add(objectGens[index]);
        UIManager.Instance.ViewResultGen();
    } 
    public void ClickDecreaseObjectGen(int index)
    {
        if (index >= 0 && index < UIManager.Instance.newobjectGens.Count)
        {
            if (UIManager.Instance.newobjectGens.Contains(objectGens[index]))
            {
                UIManager.Instance.newobjectGens.Remove(objectGens[index]);
            }
        }
        UIManager.Instance.ViewResultGen();
    }
}
