using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelButton : MonoBehaviour
{

    public LevelSelectMenu menu;
    public Sprite lockSprite;
    public Sprite defaultSprite;
    public Sprite BonusSprite;
    public Sprite HardLevelSprite;
    public Text levelText;
    
    private Button button;
    private Image image;
    int level;
    void Start()
    {
        level=PlayerPrefs.GetInt("level");
    }
  
    void OnEnable()
    {
        button=GetComponent<Button>();
        image=GetComponent<Image>();
    }
    public void SetUp(int level, bool isUnlock)
    {
        this.level=level;
        levelText.text=level.ToString();
        if(isUnlock)
        {
            image.sprite=defaultSprite;
            button.enabled=true;
            levelText.gameObject.SetActive(true);
        }
        else
        {
            if(level%6==0)
            {
                image.sprite=BonusSprite;
                button.enabled=false;
                levelText.gameObject.SetActive(false);
            }
            else if(level%15==0)
            {
                image.sprite=HardLevelSprite;
                button.enabled=false;
                levelText.gameObject.SetActive(false);
            }
            else
            {
                image.sprite=lockSprite;
                button.enabled=false;
                levelText.gameObject.SetActive(false);
            }
            
        }
    }
    public void OnClick()
    {
        menu.StartLevel(PlayerPrefs.GetInt("level"));
        // SceneManager.LoadScene("Level"+(PlayerPrefs.GetInt("level")+1).ToString());
        SceneManager.LoadScene("Level "+(levelText.text).ToString());
    }
}
