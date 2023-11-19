using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text _TimerText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _TimerText.text = string.Format("{0:D2}:{1:D2}", MapManager.instance.Min, MapManager.instance.Sec);
    }
}
