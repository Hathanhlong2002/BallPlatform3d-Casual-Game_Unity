using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom : MonoBehaviour
{
    public GameObject objectPanel;
    private bool isObjectPanelActive = false;
    [SerializeField] GameObject listObject;


    private void Start()
    {
        
    }
    private void Getlist()
    {
        for(int i=0;i<listObject.transform.childCount;i++)
        {

        }
    }
    public void SettingCustom()
    {
        isObjectPanelActive = !isObjectPanelActive;
        objectPanel.SetActive(isObjectPanelActive);
    }
}
