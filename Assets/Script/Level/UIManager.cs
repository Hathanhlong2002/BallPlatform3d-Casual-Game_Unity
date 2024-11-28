using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject dialogPanel;
    public GameObject victoryPanel;
    public GameObject objectGen;
    public Transform contentObjectGen;
    
  
    public GameObject dataResultPrefab;
    public Transform contentObjectResult;
    public List<ObjectGen> newobjectGens;
    public void OpenDialog()
    {
        dialogPanel.SetActive(true);
        victoryPanel.SetActive(false);
    }
    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void WinGame()
    {
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void ViewResultGen()
    {
        // Xóa toàn bộ các phần tử con hiện tại trong contentObjectResult
        foreach (Transform child in contentObjectResult.transform)
        {
            Destroy(child.gameObject);
        }

        // Tạo lại các phần tử con theo thứ tự trong newobjectGens
        foreach (var objGen in newobjectGens)
        {
            var genChild = Instantiate(dataResultPrefab, contentObjectResult.transform);
            var textComponent = genChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (textComponent != null)
            {
                textComponent.text = objGen.obj.name;
            }
        }
    }


}
