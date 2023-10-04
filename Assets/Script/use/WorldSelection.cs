using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WorldSelection : MonoBehaviour
{
    public GameObject[] worlds;
    public int selectWorld;
    public void NextWorld()
    {
        worlds[selectWorld].SetActive(false);
        selectWorld=(selectWorld+1)%worlds.Length;
        worlds[selectWorld].SetActive(true);
    }
    public void PreviousWorld()
    {
        worlds[selectWorld].SetActive(false);
        
        if(selectWorld<=0)
        {
            selectWorld=worlds.Length;
        }
        selectWorld--;
        worlds[selectWorld].SetActive(true);   
        
    }
    public void OKWorld()
    {
        PlayerPrefs.SetInt("SelectWorld",selectWorld);
        SceneManager.LoadScene("Start Screen");
    }
}
