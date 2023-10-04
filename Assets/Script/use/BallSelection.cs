using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallSelection : MonoBehaviour
{
    public GameObject[] ballsPlayer;
    public int selectPlayer;
    public void NextBall()
    {
        ballsPlayer[selectPlayer].SetActive(false);
        selectPlayer=(selectPlayer+1)%ballsPlayer.Length;
        ballsPlayer[selectPlayer].SetActive(true);
    }
    public void PreviousBall()
    {
        ballsPlayer[selectPlayer].SetActive(false);
        
        if(selectPlayer<=0)
        {
            selectPlayer=ballsPlayer.Length;
        }
        selectPlayer--;
        ballsPlayer[selectPlayer].SetActive(true);   
        
    }
    public void OKBall()
    {
        PlayerPrefs.SetInt("SelectBall",selectPlayer);
        SceneManager.LoadScene("Start Screen");
    }
}
