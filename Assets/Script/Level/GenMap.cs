using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMap : CustomGenMap
{

    public LevelManagerObject levelManagerObject;
    public List<LevelGen> listlevelGen=new List<LevelGen>();
    int level;
    public override void Awake()
    {
        level = PlayerPrefs.GetInt("currentLevel");
        Debug.Log("level:" + level);
        listlevelGen = levelManagerObject.levelGensList;
        if (level-1  >= listlevelGen.Count)
        {
            level = 1;
        }
        objectGenList = listlevelGen[level-1].useList;
        
    }
}
