using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    public void ResetPosition()
    {
        PlayerPrefs.DeleteKey("ColiderTrig");
        ItemCollector.instance.hearthText.text="Hearth: "+0;
    }
}
