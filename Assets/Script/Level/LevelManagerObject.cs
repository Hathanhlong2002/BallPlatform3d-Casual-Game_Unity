using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]

public class LevelManagerObject : ScriptableObject
{
    public List<LevelGen> levelGensList;

}
