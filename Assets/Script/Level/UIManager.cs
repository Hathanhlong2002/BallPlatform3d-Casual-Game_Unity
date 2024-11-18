using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject dialogPanel;
    public GameObject victoryPanel;

    public void OpenDialog()
    {
        dialogPanel.SetActive(true);
        victoryPanel.SetActive(false);
    }
    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        
    }
  
}
