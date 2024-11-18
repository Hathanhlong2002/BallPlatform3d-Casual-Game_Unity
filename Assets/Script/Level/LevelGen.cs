using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GenLevel", menuName = "ScriptableObjects/GenLevel", order = 1)]

public class LevelGen : ScriptableObject
{
    public List<ObjectGen> objectGenListSample;
    public List<ObjectGen> useList;
}
