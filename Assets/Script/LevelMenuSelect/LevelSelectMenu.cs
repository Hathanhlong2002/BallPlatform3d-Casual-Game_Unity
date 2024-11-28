using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectMenu : MonoBehaviour
{
    public int totalLevel=0;
    public int unlockedLevel=1;
    private LevelButton[] levelButtons;
    private int totalPage=0;
    private int page=0;
    private int pageItem=15;//count of number (9)
    int level;
    public GameObject nextButton;
    public GameObject backButton;
    
    void OnEnable()
    {
        levelButtons=GetComponentsInChildren<LevelButton>();
    }
    void Start()
    {
        unlockedLevel=PlayerPrefs.GetInt("unlockedLevel",1);
        level=PlayerPrefs.GetInt("level",1);
        Refresh();
    }
    public void StartLevel(int level)
    {
        
        if(level==PlayerPrefs.GetInt("unlockedLevel"))
        {
            PlayerPrefs.SetInt("unlockedLevel",PlayerPrefs.GetInt("unlockedLevel")+1 );
            Refresh();
        }
    }
    public void ClickNext()
    {
        page+=1;
        Refresh();
    }
    public void ClickBack()
    {
        page-=1;
        Refresh();
    }

    public void Refresh()
    {
        totalPage=totalLevel/pageItem;
        int index=page*pageItem;
        for(int i=0;i<levelButtons.Length;i++)
        {
            int level=index+i+1;
            if(level<=totalLevel)
            {
                levelButtons[i].gameObject.SetActive(true);
                levelButtons[i].SetUp(level,level<=unlockedLevel);
            }
            else
            {
                 levelButtons[i].gameObject.SetActive(false);
            }
        }
        CheckButton();
    }
    private void CheckButton()
    {
        backButton.SetActive(page>0);
        nextButton.SetActive(page<totalPage);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    } 
    public void CustomLevel()
    {
        SceneManager.LoadScene("CustomLevel");
    }
  
}
